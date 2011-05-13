using System;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Collections.Generic;
using iTextSharp.text;


namespace TSDTReports.Models
{

    public class ReportCell
    {
        public int colnum {get; set;}
        public int rownum {get; set;}
        public string value {get; set;}
        public int colspan {get; set;}
        public  CellType type { get; set; } 
    }

    public enum CellType
    {
        Bool, Number, String
        
    }

    public class ReportRow
    {
        public List<ReportCell> row {get; set;}
    }


    public interface IPDFReport
    {

        int PageHeight { get; set; }
        string LogoImagePath { get; set; }
        string ReportDescription { get; set; }
        float[] Headerwidths { get; set; }
        DateTime Start { get; set; }
        DateTime End { get; set; }
        string query { get; set; }
        string Name { get; set; }
        string myXML { get; set; }
        string filename { get; set; }
        string subtotal { get; set; }
        string vat { get; set; }
        string total { get; set; }
        List<string> ColList { get; set; }
        List<string> FooterList { get; set; }
        List<string> SacoList{get; set;}
        List<ReportRow> ReportRows { get; set; }
        List<tbl_Project> ProjectList { get; set; }


        void GenerateXMLReport();

        void RenderTotals();

        void OpenPDF();


        void EmailPDF(string FromAddress, string CCAddress, string ToAddress, string SubjectText, string BodyText);




    }
}
