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
    public class IlithaTimeSheetReport : IPDFReport
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
        Font ErrorFont;
        float ReportBorderWidth;

        public tbl_User User { get; set; }
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

        public IlithaTimeSheetReport(bool Landscape)
        {
            fntHeading = FontFactory.GetFont(FontFactory.HELVETICA, 7, Font.BOLD, new Color(0x00, 0x00, 0x00));
            fntDetails = FontFactory.GetFont(FontFactory.HELVETICA, 5, Font.NORMAL, new Color(0x00, 0x00, 0x00));
            footerFont = FontFactory.GetFont(FontFactory.HELVETICA, 5, Font.BOLD, new Color(0x00, 0x00, 0x00));
            SacoFont = FontFactory.GetFont(FontFactory.HELVETICA, 5, Font.BOLD, new Color(0xFF, 0xFF, 0xFF));
            ErrorFont = FontFactory.GetFont(FontFactory.HELVETICA, 5, Font.BOLD, new Color(0xFF, 0x00, 0x00));
            ReportBorderWidth = 0.5F;

            bf = BaseFont.CreateFont(BaseFont.HELVETICA_BOLD, BaseFont.CP1250, BaseFont.NOT_EMBEDDED);
            if (Landscape)
                doc = new Document(PageSize.A4.Rotate(), 18, 18, 18, 100);
            else
                doc = new Document(PageSize.A4, 18, 18, 18, 100);
            writer = PdfWriter.GetInstance(doc, ms);
            MyPageEvents events = new MyPageEvents();
            writer.PageEvent = events;
        }

        private void RenderLogo()
        {
            cb = writer.DirectContent;
            iTextSharp.text.Image Logo = iTextSharp.text.Image.GetInstance(HttpContext.Current.Server.MapPath(this.LogoImagePath));
            //Logo.ScaleToFit(200, 43);
            Logo.SetAbsolutePosition(10, 495);
            cb.AddImage(Logo);

        }


        private void RenderDescription()
        {
            cb.BeginText();
            cb.SetFontAndSize(bf, 12);
            cb.SetRGBColorFillF(0x00, 0x00, 0x00); //#636CB1
            cb.SetTextMatrix(18, 495);
            cb.ShowText("Chevron Timesheet");
            cb.EndText();
        }


        private void RenderReportJobInfo()
        {
            PdfPTable JobInfotable = new PdfPTable(2);
            JobInfotable.DefaultCell.Padding = 4;
            JobInfotable.DefaultCell.BorderWidth = ReportBorderWidth;
            float[] headerwidths = { 15, 25 }; // percentage
            JobInfotable.SetWidths(headerwidths);
            JobInfotable.WidthPercentage = 40;
            JobInfotable.HorizontalAlignment = Element.ALIGN_RIGHT;
            JobInfotable.DefaultCell.HorizontalAlignment = Element.ALIGN_LEFT;
            JobInfotable.AddCell(new Phrase("User", fntHeading));
            JobInfotable.AddCell(new Phrase(this.User.User_FirstName + " " + this.User.User_LastName + "[" + this.User.User_CAI + "]", fntHeading));
            JobInfotable.AddCell(new Phrase("Company", fntHeading));
            JobInfotable.AddCell(new Phrase(this.User.CompanyName, fntHeading));
            JobInfotable.AddCell(new Phrase("Start", fntHeading));
            JobInfotable.AddCell(new Phrase(String.Format("{0: dd-MMM-yyyy}", this.Start), fntHeading));
            JobInfotable.AddCell(new Phrase("End", fntHeading));
            JobInfotable.AddCell(new Phrase(String.Format("{0: dd-MMM-yyyy}", this.End), fntHeading));
            JobInfotable.AddCell(new Phrase("Generated", fntHeading));
            JobInfotable.AddCell(new Phrase(String.Format("{0: dd-MMM-yyyy}", DateTime.Now), fntHeading));
            JobInfotable.DefaultCell.BorderWidth = 0;
            JobInfotable.AddCell(new Phrase(" ", fntDetails));
            JobInfotable.AddCell(new Phrase(" ", fntDetails));
            JobInfotable.AddCell(new Phrase(" ", fntDetails));
            JobInfotable.AddCell(new Phrase(" ", fntDetails));
            JobInfotable.AddCell(new Phrase(" ", fntDetails));
            JobInfotable.AddCell(new Phrase(" ", fntDetails));

            doc.Add(JobInfotable);
        }

        private void RenderSignatueSection()
        {
            PdfPTable JobInfotable = new PdfPTable(2);
            JobInfotable.DefaultCell.Padding = 10;
            JobInfotable.DefaultCell.BorderWidth = ReportBorderWidth;
            float[] headerwidths = { 70, 30 }; // percentage
            JobInfotable.SetWidths(headerwidths);
            JobInfotable.WidthPercentage = 50;
            JobInfotable.HorizontalAlignment = Element.ALIGN_LEFT;
            JobInfotable.DefaultCell.HorizontalAlignment = Element.ALIGN_LEFT;
            JobInfotable.AddCell(new Phrase("SIGNATURE", fntDetails));
            JobInfotable.AddCell(new Phrase("DATE", fntDetails));
            JobInfotable.AddCell(new Phrase("CHEVRON APPROVAL", fntDetails));
            JobInfotable.AddCell(new Phrase("DATE", fntDetails));
            JobInfotable.DefaultCell.BorderWidth = 0;
            doc.Add(JobInfotable);
        }



        public void GenerateXMLReport()
        {
            try
            {
                this.doc.Open();
                RenderLogo();
                RenderDescription();
                RenderReportJobInfo();


                iTextSharp.text.Table myTable = new iTextSharp.text.Table(ColList.Count);

                myTable.Widths = this.Headerwidths;
                myTable.WidthPercentage = 100;
                //myTable.Locked = true;

                //Render Table Headers~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

                myTable.DefaultLayout.HorizontalAlignment = Element.ALIGN_LEFT;
                myTable.DefaultCell.BorderWidth = ReportBorderWidth;
                myTable.Cellpadding = 1;
                myTable.DefaultCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                myTable.DefaultCell.HorizontalAlignment = Element.ALIGN_CENTER;
                myTable.DefaultCell.BackgroundColor = Color.LIGHT_GRAY;
                myTable.DefaultCell.UseBorderPadding = true;

                foreach (var x in this.ColList)
                {
                    myTable.AddCell(new Phrase(x, fntHeading));

                }
                myTable.EndHeaders();
                //Render Details~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
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

                        if (i > ColList.Count - 3)
                            myTable.DefaultCell.BackgroundColor = Color.LIGHT_GRAY;
                        else
                            myTable.DefaultCell.BackgroundColor = Color.WHITE;

                        if (i == ColList.Count - 3)
                            myTable.AddCell(new Phrase(x.row[i].value, footerFont));
                        else
                            myTable.AddCell(new Phrase(x.row[i].value, myDetailFont));



                    }
                }

                //Footer Totals~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
                myTable.DefaultCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                myTable.DefaultCell.BackgroundColor = Color.WHITE;
                myTable.DefaultCell.Colspan = 4;
                myTable.AddCell(new Phrase("Totals", footerFont));
                myTable.DefaultCell.Colspan = 1;
                for (int i = 4; i < FooterList.Count; i++)
                {

                    myTable.AddCell(new Phrase(FooterList[i], footerFont));
                    //if ((FooterList[i].Length > 0 && SacoList[i].Length > 0) && (decimal.Parse(FooterList[i]) > decimal.Parse(SacoList[i])))
                    //{
                    //    myTable.AddCell(new Phrase(FooterList[i], ErrorFont));
                    //}
                    //else
                    //{
                    //    if (SacoList[i].Trim().Length == 0)
                    //        myTable.AddCell(new Phrase(FooterList[i], ErrorFont));
                    //    else
                    //        myTable.AddCell(new Phrase(FooterList[i], footerFont));
                    //}
                }

                //Render Saco Totals~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
                myTable.DefaultCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                myTable.DefaultCell.BackgroundColor = Color.BLUE;
                myTable.DefaultCell.Colspan = 4;
                myTable.AddCell(new Phrase("Saco Hours", SacoFont));
                myTable.DefaultCell.Colspan = 1;

                for (int i = 4; i < SacoList.Count; i++)
                {

                    myTable.AddCell(new Phrase(SacoList[i], SacoFont));
                }


                //Render Diff Totals~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
                myTable.DefaultCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                myTable.DefaultCell.BackgroundColor = Color.WHITE;
                myTable.DefaultCell.Colspan = 4;
                myTable.AddCell(new Phrase("Diff", footerFont));
                myTable.DefaultCell.Colspan = 1;
                decimal diff = 0M;
                string cellvalue = "";
                decimal sacovalue = 0M;
                decimal footervalue = 0M;

                for (int i = 4; i < SacoList.Count; i++)
                {
                    diff = 0M;
                    sacovalue = (SacoList[i].Length > 0)? decimal.Parse(SacoList[i]) : 0M;
                    footervalue = (FooterList[i].Length >0) ? decimal.Parse(FooterList[i]) : 0M;
                    diff = sacovalue - footervalue;
                    cellvalue = (diff == 0) ? string.Empty : diff.ToString("#0.00");
                    myTable.AddCell(new Phrase(cellvalue, footerFont));
                }


                //Put in a blank line~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
                //myTable.DefaultCell.BorderWidth = 0;
                //myTable.DefaultCell.BackgroundColor = Color.WHITE;

                //for (int row = 0; row < 10; row++)
                //{
                //    for (int i = 0; i < this.SacoList.Count; i++)
                //    {

                //        myTable.AddCell(new Phrase(" ", fntDetails));
                //    }
                //}

                doc.Add(myTable);

                //Render Signature Section~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
                //RenderSignatueSection();
        #endregion


            }
            catch (Exception ex)
            {
                throw (new Exception("Error: " + ex.Message));
            }
            doc.Close();

            writer.Close();
        }

        public void RenderTotals()
        {
            PdfPTable Totalstable = new PdfPTable(2);
            Totalstable.DefaultCell.Padding = 4;
            Totalstable.DefaultCell.BorderWidth = ReportBorderWidth;
            float[] Totalwidths = { 10, 10 }; // percentage
            Totalstable.SetWidths(Totalwidths);
            Totalstable.WidthPercentage = 20;
            Totalstable.HorizontalAlignment = Element.ALIGN_RIGHT;
            Totalstable.DefaultCell.HorizontalAlignment = Element.ALIGN_LEFT;
            Totalstable.DefaultCell.BorderWidth = 0;
            Totalstable.AddCell(new Phrase(" ", fntDetails));
            Totalstable.AddCell(new Phrase(" ", fntDetails));
            Totalstable.DefaultCell.BorderWidth = ReportBorderWidth;
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
            throw new NotImplementedException();
        }


    }
}
