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




public class UserReport
{
    MemoryStream ms = new MemoryStream();
    Document doc;
    PdfWriter writer;
    PdfContentByte cb;
    int PageHeight = 595;
    Font fntHeading = FontFactory.GetFont(FontFactory.HELVETICA, 7, Font.BOLD, new Color(0x00, 0x00, 0x00));
    Font fntDetails = FontFactory.GetFont(FontFactory.HELVETICA, 7, Font.NORMAL, new Color(0x00, 0x00, 0x00));
    BaseFont bf = BaseFont.CreateFont(BaseFont.HELVETICA_BOLD, BaseFont.CP1250, BaseFont.NOT_EMBEDDED);
    string ImagePath = "~/images/img_hallmark.gif.gif";

    public string JobNumber { get; set; }
    public string DateCreated { get; set; }
    public GridView myGridview;
    public string Publication { get; set; }
    public string User { get; set; }
    public string Issue { get; set; }
    public string Year { get; set; }
    public string SummaryTotal { get; set; }
    public string VatTotal { get; set; }
    public string GrandTotal { get; set; }
    public string ReportName { get; set; }


    public UserReport()
    {
        doc = new Document(PageSize.A4.Rotate(), 35, 35, 35, 35);
        writer = PdfWriter.GetInstance(doc, ms);
        MyPageEvents events = new MyPageEvents();
        writer.PageEvent = events;
    }

    #region StyleReport - Template

    private void RenderLogo()
    {
        cb = writer.DirectContent;
        iTextSharp.text.Image Logo = iTextSharp.text.Image.GetInstance(HttpContext.Current.Server.MapPath(ImagePath));
        Logo.ScaleToFit(200, 83);
        Logo.SetAbsolutePosition(35, PageHeight - 90);
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

    private void RenderDescription(string Description)
    {
        cb.BeginText();
        cb.SetFontAndSize(bf, 10);
        cb.SetRGBColorFillF(0x00, 0x00, 0x00); //#636CB1
        cb.SetTextMatrix(60, PageHeight - 160);
        cb.ShowText(Description);
        cb.EndText();
    }

    private void RenderReportJobInfo()
    {

        PdfPTable JobInfotable = new PdfPTable(2);
        JobInfotable.DefaultCell.Padding = 4;
        JobInfotable.DefaultCell.BorderWidth = 1;
        float[] headerwidths = { 10, 25 }; // percentage
        JobInfotable.SetWidths(headerwidths);
        JobInfotable.WidthPercentage = 30;
        JobInfotable.HorizontalAlignment = Element.ALIGN_RIGHT;
        JobInfotable.DefaultCell.HorizontalAlignment = Element.ALIGN_LEFT;
        JobInfotable.AddCell(new Phrase("Job Number", fntDetails));
        JobInfotable.AddCell(new Phrase(this.JobNumber, fntDetails));
        JobInfotable.AddCell(new Phrase("Date Created", fntDetails));
        JobInfotable.AddCell(new Phrase(this.DateCreated, fntDetails));
        JobInfotable.AddCell(new Phrase("Publication", fntDetails));
        JobInfotable.AddCell(new Phrase(this.Publication, fntDetails));
        JobInfotable.AddCell(new Phrase("Issue", fntDetails));
        JobInfotable.AddCell(new Phrase(this.Issue, fntDetails));
        JobInfotable.AddCell(new Phrase("Year", fntDetails));
        JobInfotable.AddCell(new Phrase(this.Year, fntDetails));
        JobInfotable.AddCell(new Phrase("Sent By", fntDetails));
        JobInfotable.AddCell(new Phrase(this.User, fntDetails));
        JobInfotable.DefaultCell.BorderWidth = 0;
        JobInfotable.AddCell(new Phrase(" ", fntDetails));
        JobInfotable.AddCell(new Phrase(" ", fntDetails));
        JobInfotable.AddCell(new Phrase(" ", fntDetails));
        doc.Add(JobInfotable);
    }

    public void StyleDetailHeader(ref iTextSharp.text.Table JobDetailstable, float[] DetailsHeaderwidths)
    {
        JobDetailstable.Padding = 1;
        JobDetailstable.DefaultCell.BorderWidth = 1;
        JobDetailstable.Widths = DetailsHeaderwidths;
        JobDetailstable.WidthPercentage = 100;
        JobDetailstable.DefaultCell.HorizontalAlignment = Element.ALIGN_CENTER;
        JobDetailstable.DefaultCell.BackgroundColor = Color.LIGHT_GRAY;
    }

    #endregion

    public void OpenPDF(string filename)
    {
        HttpContext.Current.Response.ContentType = "application/pdf";
        HttpContext.Current.Response.AddHeader("content-disposition", "attachment;filename=" + filename + ".pdf");
        HttpContext.Current.Response.OutputStream.Write(ms.GetBuffer(), 0, ms.GetBuffer().Length);
        HttpContext.Current.Response.OutputStream.Flush();
        HttpContext.Current.Response.End();
    }

    //public void EmailPDF(string filename, string FromAddress, string CCAddress, string ToAddress, string SubjectText, string BodyText)
    //{


    //    MemoryStream s = new MemoryStream(ms.GetBuffer(), 0, ms.GetBuffer().Length);
    //    Attachment data = new Attachment(s, filename, "application/pdf");


    //    using (MailMessage myEmail = new MailMessage(FromAddress, ToAddress, SubjectText, BodyText))
    //    {
    //        if (CCAddress != "0")
    //            myEmail.CC.Add(new MailAddress(CCAddress));
    //        myEmail.IsBodyHtml = true;

    //        System.Net.Mail.Attachment mypdfAttachment = data;
    //        myEmail.Attachments.Add(mypdfAttachment);

    //        SmtpClient client = new SmtpClient();
    //        client.Host = System.Configuration.ConfigurationManager.AppSettings["SMTPSERVER"];

    //        try
    //        {
    //            client.Send(myEmail);
    //            MessageBox.Show("Email Sent Successfully");
    //        }
    //        catch (SmtpException ex)
    //        {
    //            MessageBox.Show("Error: " + ex.Message);

    //        }

    //    }

    //    //Email myEmail = new Email(FromAddress, CCAddress, ToAddress, SubjectText, BodyText);
    //    //myEmail.send();
    //}




    public void GenerateReport(string query, float[] Headerwidths)
    {
        try
        {

            doc.Open();
            RenderLogo();
            RenderHeaderAddress();
            RenderReportJobInfo();
            RenderDescription(this.ReportName + " " + query);

            #region Add Details Table
            PdfPTable JobDetailstable = new PdfPTable(8);
            JobDetailstable.DefaultCell.Padding = 2;
            JobDetailstable.DefaultCell.BorderWidth = 1;
            float[] DetailsHeaderwidths = { 10, 10, 10, 10, 30, 10, 10, 10 }; // percentage
            JobDetailstable.SetWidths(DetailsHeaderwidths);
            JobDetailstable.WidthPercentage = 100;
            JobDetailstable.DefaultCell.HorizontalAlignment = Element.ALIGN_CENTER;
            JobDetailstable.DefaultCell.BackgroundColor = Color.LIGHT_GRAY;
            JobDetailstable.AddCell(new Phrase("Date", fntHeading));
            JobDetailstable.AddCell(new Phrase("Hrs", fntHeading));
            JobDetailstable.AddCell(new Phrase("Item", fntHeading));
            JobDetailstable.AddCell(new Phrase("Item Cost", fntHeading));
            JobDetailstable.AddCell(new Phrase("Description", fntHeading));
            JobDetailstable.AddCell(new Phrase("Total(R)", fntHeading));
            JobDetailstable.AddCell(new Phrase("Markup", fntHeading));
            JobDetailstable.AddCell(new Phrase("New Total(R)", fntHeading));

            for (int i = 0; i < myGridview.Rows.Count; i++)
            {
                JobDetailstable.DefaultCell.BackgroundColor = Color.WHITE;
                JobDetailstable.DefaultCell.HorizontalAlignment = Element.ALIGN_LEFT;
                JobDetailstable.AddCell(new Phrase(((Label)myGridview.Rows[i].FindControl("lblDateAdded")).Text, fntDetails));
                JobDetailstable.DefaultCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                JobDetailstable.AddCell(new Phrase(((Label)myGridview.Rows[i].FindControl("lblHrs")).Text, fntDetails));
                JobDetailstable.DefaultCell.HorizontalAlignment = Element.ALIGN_LEFT;
                JobDetailstable.AddCell(new Phrase(((Label)myGridview.Rows[i].FindControl("lblItem")).Text, fntDetails));
                JobDetailstable.DefaultCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                JobDetailstable.AddCell(new Phrase(((Label)myGridview.Rows[i].FindControl("lblItemAmount")).Text, fntDetails));
                JobDetailstable.DefaultCell.HorizontalAlignment = Element.ALIGN_LEFT;
                JobDetailstable.AddCell(new Phrase(((Label)myGridview.Rows[i].FindControl("lbldescription")).Text, fntDetails));
                JobDetailstable.DefaultCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                JobDetailstable.AddCell(new Phrase(((Label)myGridview.Rows[i].FindControl("lblTotal")).Text, fntDetails));
                JobDetailstable.AddCell(new Phrase(((Label)myGridview.Rows[i].FindControl("lblMarkup")).Text, fntDetails));
                JobDetailstable.AddCell(new Phrase(((Label)myGridview.Rows[i].FindControl("lblNewTotal")).Text, fntDetails));
            }

            #region Footer Totals
            JobDetailstable.DefaultCell.BackgroundColor = Color.LIGHT_GRAY;
            JobDetailstable.DefaultCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            JobDetailstable.AddCell(new Phrase(""));
            JobDetailstable.AddCell(new Phrase(((Label)myGridview.FooterRow.FindControl("lblTotalHrs")).Text, fntDetails));
            JobDetailstable.AddCell(new Phrase(""));
            JobDetailstable.AddCell(new Phrase(""));
            JobDetailstable.AddCell(new Phrase(""));
            JobDetailstable.AddCell(new Phrase(((Label)myGridview.FooterRow.FindControl("lblTotalTotal")).Text, fntDetails));
            JobDetailstable.AddCell(new Phrase(""));
            JobDetailstable.AddCell(new Phrase(((Label)myGridview.FooterRow.FindControl("lblTotalNewTotal")).Text, fntDetails));
            #endregion


            doc.Add(JobDetailstable);


            #endregion

            #region Totals


            PdfPTable Totalstable = new PdfPTable(2);
            Totalstable.DefaultCell.Padding = 2;
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
            Totalstable.AddCell(new Phrase(this.SummaryTotal, fntDetails));
            Totalstable.DefaultCell.BackgroundColor = Color.LIGHT_GRAY;
            Totalstable.DefaultCell.HorizontalAlignment = Element.ALIGN_LEFT;
            Totalstable.AddCell(new Phrase("Vat", fntHeading));
            Totalstable.DefaultCell.BackgroundColor = Color.WHITE;
            Totalstable.DefaultCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            Totalstable.AddCell(new Phrase(this.VatTotal, fntDetails));
            Totalstable.DefaultCell.BackgroundColor = Color.LIGHT_GRAY;
            Totalstable.DefaultCell.HorizontalAlignment = Element.ALIGN_LEFT;
            Totalstable.AddCell(new Phrase("Grand Total", fntHeading));
            Totalstable.DefaultCell.BackgroundColor = Color.WHITE;
            Totalstable.DefaultCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            Totalstable.AddCell(new Phrase(this.GrandTotal, fntDetails));

            doc.Add(Totalstable);
            #endregion


        }
        catch (Exception ex)
        {
            throw(new Exception("Cannot display report" ));
        }
        doc.Close();

        writer.Close();
    }

}
