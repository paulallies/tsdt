using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.html;
using iTextSharp.text.xml;
using System.Xml;
using System.IO;
using System.Net.Mail;
using System.Linq;
using System.Xml.Linq;

namespace TSDTReports.Models
{
    public class TimeSheetReport : IPDFReport
    {
        #region IPDFReport Members


        MemoryStream ms = new MemoryStream();
        public Document doc { get; set; }
        public int PageHeight { get; set; }
        PdfWriter writer;
        PdfContentByte cb;
        BaseFont bf;
        Font fntHeading;
        Font fntDetails;
        Font footerFont;
        Font SacoFont;

        public string Name { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public string LogoImagePath { get; set; }
        public string ReportDescription { get; set; }
        public float[] Headerwidths { get; set; }
        public string query { get; set; }
        public string myXML { get; set; }
        public string filename { get; set; }
        public string subtotal { get; set; }
        public string vat { get; set; }
        public string total { get; set; }

        public List<string> ColList { get; set; }
        public List<string> FooterList { get; set; }
        public List<string> SacoList { get; set; }
        public List<ReportRow> ReportRows { get; set; }
        public List<tbl_Project> ProjectList { get; set; }



        public TimeSheetReport(bool Landscape)
        {
            fntHeading = FontFactory.GetFont(FontFactory.HELVETICA, 7, Font.BOLD, new Color(0x00, 0x00, 0x00));
            fntDetails = FontFactory.GetFont(FontFactory.HELVETICA, 5, Font.NORMAL, new Color(0x00, 0x00, 0x00));
            footerFont = FontFactory.GetFont(FontFactory.HELVETICA, 5, Font.BOLD, new Color(0x00, 0x00, 0x00));
            SacoFont = FontFactory.GetFont(FontFactory.HELVETICA, 5, Font.BOLD, new Color(0xFF, 0xFF, 0xFF));

            bf = BaseFont.CreateFont(BaseFont.HELVETICA_BOLD, BaseFont.CP1250, BaseFont.NOT_EMBEDDED);
            if (Landscape)
                doc = new Document(PageSize.A4.Rotate(), 18, 18, 18, 18);
            else
                doc = new Document(PageSize.A4, 18, 18, 18, 18);
            writer = PdfWriter.GetInstance(doc, ms);
            MyPageEvents events = new MyPageEvents();
            writer.PageEvent = events;
        }

        private void RenderLogo()
        {
            cb = writer.DirectContent;
            iTextSharp.text.Image Logo = iTextSharp.text.Image.GetInstance(HttpContext.Current.Server.MapPath(this.LogoImagePath));
            //Logo.ScaleToFit(200, 43);
            Logo.SetAbsolutePosition(15, 470);
            cb.AddImage(Logo);

        }

        private void RenderHeaderAddress()
        {
            PdfPTable table = new PdfPTable(1);
            table.HorizontalAlignment = Element.ALIGN_RIGHT;
            table.DefaultCell.BorderWidth = 0;
            table.DefaultCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            table.DefaultCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            table.AddCell(new Phrase("", fntDetails));
            table.AddCell(new Phrase("", fntDetails));
            table.AddCell(new Phrase("", fntDetails));
            table.AddCell(new Phrase("", fntDetails));
            table.AddCell(new Phrase("HEAD OFFICE: 1st Floor, 70 Prestwich Street, Cape Town 8001", fntDetails));
            table.AddCell(new Phrase("P.O. Box 440, Green Point, 8051", fntDetails));
            table.AddCell(new Phrase("Tel: (021) 417 1111 Fax: (021) 417 1112", fntDetails));
            table.AddCell(new Phrase("Email: newmedia@newmediapub.co.za", fntDetails));
            table.AddCell(new Phrase("JHB OFFICE: 5a Protea Place, Protea Park, Santon, 2146", fntDetails));
            table.AddCell(new Phrase("Tel: (011) 263 4784 Fax: (011) 883 1337", fntDetails));
            table.AddCell(new Phrase("Vat Registration No: 4570172611", fntDetails));
            table.AddCell(new Phrase("", fntDetails));
            table.AddCell(new Phrase("", fntDetails));
            table.AddCell(new Phrase("", fntDetails));
            table.AddCell(new Phrase("", fntDetails));
            table.AddCell(new Phrase("", fntDetails));
            table.AddCell(new Phrase("", fntDetails));
            table.AddCell(new Phrase("", fntDetails));
            table.AddCell(new Phrase("", fntDetails));
            table.AddCell(new Phrase("", fntDetails));
            table.AddCell(new Phrase("", fntDetails));
            doc.Add(table);

        }

        private void RenderDescription()
        {
            cb.BeginText();
            cb.SetFontAndSize(bf, 10);
            cb.SetRGBColorFillF(0x00, 0x00, 0x00); //#636CB1
            cb.SetTextMatrix(60, PageHeight - 160);
            cb.ShowText(this.ReportDescription);
            cb.EndText();
        }


        private void RenderReportJobInfo()
        {
            PdfPTable JobInfotable = new PdfPTable(2);
            JobInfotable.DefaultCell.Padding = 4;
            JobInfotable.DefaultCell.BorderWidth = 1;
            float[] headerwidths = { 15, 25 }; // percentage
            JobInfotable.SetWidths(headerwidths);
            JobInfotable.WidthPercentage = 40;
            JobInfotable.HorizontalAlignment = Element.ALIGN_RIGHT;
            JobInfotable.DefaultCell.HorizontalAlignment = Element.ALIGN_LEFT;
            JobInfotable.AddCell(new Phrase("User", fntHeading));
            JobInfotable.AddCell(new Phrase(this.Name, fntDetails));
            JobInfotable.AddCell(new Phrase("Start", fntHeading));
            JobInfotable.AddCell(new Phrase(String.Format("{0: dd-MMM-yyyy}", this.Start), fntDetails));
            JobInfotable.AddCell(new Phrase("End", fntHeading));
            JobInfotable.AddCell(new Phrase(String.Format("{0: dd-MMM-yyyy}", this.End), fntDetails));
            JobInfotable.AddCell(new Phrase("Generated", fntHeading));
            JobInfotable.AddCell(new Phrase(String.Format("{0: dd-MMM-yyyy}", DateTime.Now), fntDetails));
            JobInfotable.DefaultCell.BorderWidth = 0;
            JobInfotable.AddCell(new Phrase(" ", fntDetails));
            JobInfotable.AddCell(new Phrase(" ", fntDetails));
            JobInfotable.AddCell(new Phrase(" ", fntDetails));
            JobInfotable.AddCell(new Phrase(" ", fntDetails));
            JobInfotable.AddCell(new Phrase(" ", fntDetails));
            JobInfotable.AddCell(new Phrase(" ", fntDetails));

            doc.Add(JobInfotable);
        }



        public void GenerateXMLReport()
        {
            try
            {
                this.doc.Open();
                RenderLogo();
                //RenderHeaderAddress();
                RenderReportJobInfo();
    
                #region Add Details Table
                PdfPTable myTable = new PdfPTable(ColList.Count );
                myTable.SetTotalWidth(this.Headerwidths);
                myTable.LockedWidth = true;
                myTable.HorizontalAlignment = Element.ALIGN_LEFT;
                myTable.DefaultCell.BorderWidth = 1;
                myTable.DefaultCell.HorizontalAlignment = Element.ALIGN_CENTER;
                myTable.DefaultCell.BackgroundColor = Color.LIGHT_GRAY;

                foreach (var x in this.ColList)
                {
                    myTable.AddCell(new Phrase(x, fntHeading));

                }
                //myTable.EndHeaders();
                Font myDetailFont = fntDetails;
                foreach (var x in this.ReportRows)
                {
                    for (int i = 0; i < ColList.Count; i++)
                    {
                        myTable.DefaultCell.BackgroundColor = Color.WHITE;
                        if (x.row[i].type == CellType.Number)
                            myTable.DefaultCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                        else
                            myTable.DefaultCell.HorizontalAlignment = Element.ALIGN_LEFT;
                        myTable.AddCell(new Phrase(x.row[i].value, myDetailFont));
                    }
                }

                myTable.DefaultCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                myTable.DefaultCell.BackgroundColor = Color.LIGHT_GRAY;
                int colindex = 0;
                foreach (var x in this.FooterList)
                {
                    //decimal.Parse(FooterList[colindex]) > 
                    myTable.AddCell(new Phrase(x, footerFont));
                    colindex++;
                }
                myTable.DefaultCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                myTable.DefaultCell.BackgroundColor = Color.BLUE;
                

                foreach (var x in this.SacoList)
                {
                    myTable.AddCell(new Phrase(x, SacoFont));
                }

                doc.Add(myTable);

                #endregion


            }
            catch (Exception ex)
            {
                //MessageBox.Show("Error: " + ex.Message);
            }
            doc.Close();

            writer.Close();
        }

        public void RenderTotals()
        {
            PdfPTable Totalstable = new PdfPTable(2);
            Totalstable.DefaultCell.Padding = 4;
            Totalstable.DefaultCell.BorderWidth = 1;
            float[] Totalwidths = { 10, 10 }; // percentage
            Totalstable.SetWidths(Totalwidths);
            Totalstable.WidthPercentage = 20;
            Totalstable.HorizontalAlignment = Element.ALIGN_RIGHT;
            Totalstable.DefaultCell.HorizontalAlignment = Element.ALIGN_LEFT;
            Totalstable.DefaultCell.BorderWidth = 0;
            Totalstable.AddCell(new Phrase(" ", fntDetails));
            Totalstable.AddCell(new Phrase(" ", fntDetails));
            Totalstable.DefaultCell.BorderWidth = 1;
            Totalstable.DefaultCell.BackgroundColor = Color.LIGHT_GRAY;
            Totalstable.AddCell(new Phrase("SubTotal", fntHeading));
            Totalstable.DefaultCell.BackgroundColor = Color.WHITE;
            Totalstable.DefaultCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            Totalstable.AddCell(new Phrase(this.subtotal, fntDetails));
            Totalstable.DefaultCell.BackgroundColor = Color.LIGHT_GRAY;
            Totalstable.DefaultCell.HorizontalAlignment = Element.ALIGN_LEFT;
            Totalstable.AddCell(new Phrase("Vat", fntHeading));
            Totalstable.DefaultCell.BackgroundColor = Color.WHITE;
            Totalstable.DefaultCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            Totalstable.AddCell(new Phrase(this.vat, fntDetails));
            Totalstable.DefaultCell.BackgroundColor = Color.LIGHT_GRAY;
            Totalstable.DefaultCell.HorizontalAlignment = Element.ALIGN_LEFT;
            Totalstable.AddCell(new Phrase("Grand Total", fntHeading));
            Totalstable.DefaultCell.BackgroundColor = Color.WHITE;
            Totalstable.DefaultCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            Totalstable.AddCell(new Phrase(this.total, fntDetails));
            doc.Add(Totalstable);
        }

        public void OpenPDF()
        {
            HttpContext.Current.Response.ContentType = "application/pdf";
            HttpContext.Current.Response.AddHeader("content-disposition", "attachment;filename=" + this.filename + ".pdf");
            HttpContext.Current.Response.OutputStream.Write(ms.GetBuffer(), 0, ms.GetBuffer().Length);
            HttpContext.Current.Response.OutputStream.Flush();
            HttpContext.Current.Response.End();
        }

        public void EmailPDF(string FromAddress, string CCAddress, string ToAddress, string SubjectText, string BodyText)
        {

            MemoryStream s = new MemoryStream(ms.GetBuffer(), 0, ms.GetBuffer().Length);
            Attachment data = new Attachment(s, filename, "application/pdf");

            using (MailMessage myEmail = new MailMessage(FromAddress, ToAddress, SubjectText, BodyText))
            {
                if (CCAddress != String.Empty)
                    myEmail.CC.Add(new MailAddress(CCAddress));
                myEmail.IsBodyHtml = true;

                System.Net.Mail.Attachment mypdfAttachment = data;
                myEmail.Attachments.Add(mypdfAttachment);

                SmtpClient client = new SmtpClient();
                client.Host = System.Configuration.ConfigurationManager.AppSettings["SMTPSERVER"];

                try
                {
                    client.Send(myEmail);
                    //  MessageBox.Show("Email Sent Successfully");
                }
                catch (SmtpException ex)
                {
                    //MessageBox.Show("Error: " + ex.Message);

                }

            }
        }

        #endregion





    }
}
