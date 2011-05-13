using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using TSDTReports.Models;
using iTextSharp.text;
using System.Text;
using System.IO;
using NPOI.HSSF.UserModel;
using NPOI.HSSF.Util;


namespace TSDTReports.Controllers 
{
    public class ReportController : Controller
    {
        IUserRepository userdb = new UserRepository();
        ITimesheetRepository timedb = new TimeSheetRepository();
        IRoleRepository roledb = new RoleRepository();

        public ActionResult Index()
        {
            string role = roledb.GetUserRolesByUser(User.Identity.Name.ToString().ToUpper().Replace("CT\\", ""))[0];
            ViewData["Role"] = role;

            DateTime myDate = DateTime.Now;

            DateTime firstDayOfThisMonth = myDate.Subtract(TimeSpan.FromDays(myDate.Day - 1));
            DateTime firstDayOfNextMonth = firstDayOfThisMonth.AddMonths(1);
            DateTime lastDayOfThisMonth = firstDayOfNextMonth.Subtract(TimeSpan.FromDays(1));

            string User_CAI = "";
                User_CAI = User.Identity.Name.ToUpper().Replace("CT\\", "");
            //User_CAI = "PALX";
            ViewData["Users"] = new SelectList(userdb.GetUserItems(), "cai", "detail", User_CAI);
            ViewData["MonthDays"] = MonthDays(myDate);
            
            return View(timedb.GetReportByUser(User_CAI, firstDayOfThisMonth, lastDayOfThisMonth));

        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Index(FormCollection coll)
        {
            DateTime myDate = DateTime.Now;

            string role = roledb.GetUserRolesByUser(User.Identity.Name.ToString().ToUpper().Replace("CT\\", ""))[0];
            ViewData["Role"] = role;

            DateTime firstDayOfThisMonth = DateTime.Parse(coll["txtDateFrom"]) ;
            DateTime lastDayOfThisMonth = DateTime.Parse(coll["txtDateTo"]);

            switch(coll["submit"])
            {
                case "Get Report":            
                    ViewData["Users"] = new SelectList(userdb.GetUserItems(), "cai", "detail", coll["ddlUsers"]);
                    ViewData["MonthDays"] = lastDayOfThisMonth.Subtract(firstDayOfThisMonth).Days + 1 ;
                    break;
                case "Print":
                    GenerateIlithaPDF(timedb.GetReportByUser(coll["ddlUsers"], firstDayOfThisMonth, lastDayOfThisMonth));
                    break;
            }

            return View(timedb.GetReportByUser(coll["ddlUsers"], firstDayOfThisMonth, lastDayOfThisMonth));


        }

        private FileContentResult SendToExcel(FormCollection coll)
        {
            string excelFileStringContent = DownloadReportHTMLContent(coll["txtDateFrom"], coll["txtDateTo"], coll["ddlUsers"]);
            byte[] excelFileBytesContent = this.Response.ContentEncoding.GetBytes(excelFileStringContent);
            FileContentResult excelFileContentResult = new FileContentResult(excelFileBytesContent, "application/vnd.ms-excel");
            return excelFileContentResult;
        }

        private FileContentResult SendToExcel2(FormCollection coll)
        {
            DateTime firstDay = DateTime.Parse(coll["txtDateFrom"]);
            DateTime lastDay = DateTime.Parse(coll["txtDateTo"]);
            string User_CAI = coll["ddlUsers"];
            ReportPackage rp = timedb.GetReportByUser(User_CAI, firstDay, lastDay);

           // FileStream fs = new FileStream(Server.MapPath("~/Content/Template.xls"), FileMode.Open, FileAccess.Read);

            // Getting the complete workbook...
            HSSFWorkbook wb = new HSSFWorkbook();

            // Getting the worksheet by its name...
            HSSFSheet sheet = wb.CreateSheet(User_CAI+"_"+coll["txtDateFrom"] +"_" + coll["txtDateTo"]);

            sheet.SetColumnWidth(2, 20 * 256);
            sheet.SetColumnWidth(3, 20 * 256);
            sheet.SetColumnWidth(4, 30 * 256);

            
            HSSFFont font1 = wb.CreateFont();
            font1.Boldweight = HSSFFont.BOLDWEIGHT_BOLD;
            font1.Color = HSSFColor.BLACK.index;
            font1.FontHeightInPoints = 10;

            HSSFFont font2 = wb.CreateFont();
            font2.Color = HSSFColor.BLACK.index;
            font2.FontHeightInPoints = 8;

            HSSFFont sacofont = wb.CreateFont();
            sacofont.Color = HSSFColor.WHITE.index;
            sacofont.FontHeightInPoints = 8;

            HSSFFont font2Bold = wb.CreateFont();
            font2Bold = font2;
            font2Bold.Boldweight = HSSFFont.BOLDWEIGHT_BOLD;

            HSSFFont font_Red_Bold = wb.CreateFont();
            font_Red_Bold.Color = HSSFColor.RED.index;
            font_Red_Bold.Boldweight = HSSFFont.BOLDWEIGHT_BOLD;
            font_Red_Bold.FontHeightInPoints = 10;
            
            HSSFCellStyle globalstyle = wb.CreateCellStyle();
            globalstyle.BorderBottom = HSSFCellStyle.BORDER_THIN;
            globalstyle.BottomBorderColor = HSSFColor.BLACK.index;
            globalstyle.BorderLeft = HSSFCellStyle.BORDER_THIN;
            globalstyle.LeftBorderColor = HSSFColor.BLACK.index;
            globalstyle.BorderRight = HSSFCellStyle.BORDER_THIN;
            globalstyle.RightBorderColor = HSSFColor.BLACK.index;
            globalstyle.BorderTop = HSSFCellStyle.BORDER_THIN;
            globalstyle.TopBorderColor = HSSFColor.BLACK.index;


            HSSFCellStyle headerstyle = wb.CreateCellStyle();
            headerstyle.CloneStyleFrom(globalstyle);
            headerstyle.Alignment = HSSFCellStyle.ALIGN_CENTER;
            headerstyle.FillForegroundColor = HSSFColor.GREY_25_PERCENT.index;
            headerstyle.FillPattern = HSSFCellStyle.SOLID_FOREGROUND;
            headerstyle.SetFont(font1);

            HSSFCellStyle dayStyle = wb.CreateCellStyle();
            dayStyle.CloneStyleFrom(headerstyle);
            dayStyle.SetFont(font1);
            
            HSSFCellStyle datastyle = wb.CreateCellStyle();
            datastyle.CloneStyleFrom(globalstyle);
            datastyle.SetFont(font2);

            HSSFCellStyle footerStyle = wb.CreateCellStyle();
            footerStyle.CloneStyleFrom(headerstyle);
            footerStyle.Alignment = HSSFCellStyle.ALIGN_RIGHT;
            footerStyle.SetFont(font2);

            HSSFCellStyle ErrorStyle = wb.CreateCellStyle();
            ErrorStyle.CloneStyleFrom(headerstyle);
            ErrorStyle.Alignment = HSSFCellStyle.ALIGN_RIGHT;
            ErrorStyle.SetFont(font_Red_Bold);

            HSSFCellStyle SacofooterStyle = wb.CreateCellStyle();
            SacofooterStyle.CloneStyleFrom(globalstyle);
            SacofooterStyle.Alignment = HSSFCellStyle.ALIGN_RIGHT;
            SacofooterStyle.FillForegroundColor = HSSFColor.LIGHT_BLUE.index;
            SacofooterStyle.FillPattern = HSSFCellStyle.SOLID_FOREGROUND;
            SacofooterStyle.SetFont(sacofont);

            //Header
            HSSFRow row = sheet.CreateRow(0);

            row.CreateCell(0).CellStyle = headerstyle;
            row.GetCell(0).SetCellValue("Status");

            row.CreateCell(1).CellStyle = headerstyle;
            row.GetCell(1).SetCellValue("Prj No.");

            row.CreateCell(2).CellStyle = headerstyle;
            row.GetCell(2).SetCellValue("SAP");

           row.CreateCell(3).CellStyle = headerstyle;
            row.GetCell(3).SetCellValue("Cost Code");

            row.CreateCell(4).CellStyle = headerstyle;

            row.GetCell(4).SetCellValue("Project Name");

            int colcount = 5;
            for (DateTime dt = rp.DateFrom; dt < rp.DateTo.AddDays(1); dt = dt.AddDays(1))
            {
                row.CreateCell(colcount).CellStyle = dayStyle;
                row.GetCell(colcount).CellStyle.WrapText = true;
                row.GetCell(colcount).SetCellValue(new DateTime(dt.Year, dt.Month, dt.Day).ToString("ddd") +"\n"+ new DateTime(dt.Year, dt.Month, dt.Day).ToString("dd") + "\n" + new DateTime(dt.Year, dt.Month, dt.Day).ToString("MMM"));

                colcount++;
            }
            row.CreateCell(colcount).CellStyle = headerstyle;
            row.GetCell(colcount).SetCellValue("Total");

            //Details
            int rowcount = 1;
            foreach (var ts in rp.TimeSheets)
            {
               sheet.CreateRow(rowcount);
               sheet.GetRow(rowcount).CreateCell(0).CellStyle = datastyle;
               sheet.GetRow(rowcount).GetCell(0).SetCellValue(ts.Status);

               sheet.GetRow(rowcount).CreateCell(1).CellStyle = datastyle;
               sheet.GetRow(rowcount).GetCell(1).SetCellValue(ts.Project_Number.ToString());

               sheet.GetRow(rowcount).CreateCell(2).CellStyle = datastyle;
               sheet.GetRow(rowcount).GetCell(2).SetCellValue(ts.SAP);

               sheet.GetRow(rowcount).CreateCell(3).CellStyle = datastyle;
               sheet.GetRow(rowcount).GetCell(3).SetCellValue(ts.WBS);

               sheet.GetRow(rowcount).CreateCell(4).CellStyle = datastyle;
               sheet.GetRow(rowcount).GetCell(4).CellStyle.WrapText = true;
               sheet.GetRow(rowcount).GetCell(4).SetCellValue(ts.Project_Name);

                colcount = 5;
                foreach (var d in ts.Days)
               {
                   sheet.GetRow(rowcount).CreateCell(colcount).CellStyle = datastyle;
                   if (d.HoursWorked.HasValue) 
                       sheet.GetRow(rowcount).GetCell(colcount).SetCellValue((double)d.HoursWorked.Value);
                   else
                       sheet.GetRow(rowcount).GetCell(colcount).SetCellValue(string.Empty);                      
                   colcount++;
               }
                sheet.GetRow(rowcount).CreateCell(colcount).CellStyle = datastyle;
                sheet.GetRow(rowcount).GetCell(colcount).SetCellValue((double)ts.Total);
                rowcount++;
            }

            //Footer
            //Insert blank spaces
            sheet.CreateRow(rowcount);
            sheet.GetRow(rowcount).CreateCell(0).CellStyle = footerStyle;
            sheet.GetRow(rowcount).CreateCell(1).CellStyle = footerStyle;
            sheet.GetRow(rowcount).CreateCell(2).CellStyle = footerStyle;
            sheet.GetRow(rowcount).CreateCell(3).CellStyle = footerStyle;
            sheet.GetRow(rowcount).CreateCell(4).CellStyle = footerStyle;

            colcount = 5; //Start at column 5 inserting values
            int loopindex = 0;
            foreach(var d in rp.DayTotals)
             {
                 sheet.GetRow(rowcount).CreateCell(colcount).CellStyle = footerStyle;
                 if (d.HoursWorked.HasValue && (decimal)d.HoursWorked.Value > 0)
                 {
                     loopindex = d.Number-1;
                     if (rp.DayTotals[loopindex].HoursWorked.Value > rp.DaySacoTotals[loopindex].HoursWorked.Value)
                     {
                         sheet.GetRow(rowcount).GetCell(colcount).CellStyle = ErrorStyle;
                     }
                     sheet.GetRow(rowcount).GetCell(colcount).SetCellValue(((decimal)d.HoursWorked.Value).ToString("#0.00"));
                 }
                 else
                     sheet.GetRow(rowcount).GetCell(colcount).SetCellValue(string.Empty);   
                colcount++;
             }
            sheet.GetRow(rowcount).CreateCell(colcount).CellStyle = footerStyle;
            sheet.GetRow(rowcount).GetCell(colcount).SetCellValue(((decimal)rp.GrandTotal).ToString("#0.00"));

            rowcount++;
            //SACO Footer
            //Insert blank spaces
            sheet.CreateRow(rowcount);
            sheet.GetRow(rowcount).CreateCell(0).CellStyle = SacofooterStyle;
            sheet.GetRow(rowcount).GetCell(0).SetCellValue("Saco");
            sheet.GetRow(rowcount).CreateCell(1).CellStyle = SacofooterStyle;
            sheet.GetRow(rowcount).CreateCell(2).CellStyle = SacofooterStyle;
            sheet.GetRow(rowcount).CreateCell(3).CellStyle = SacofooterStyle;
            sheet.GetRow(rowcount).CreateCell(4).CellStyle = SacofooterStyle;

            colcount = 5; //Start at column 5 inserting values
            foreach (var d in rp.DaySacoTotals)
            {
                sheet.GetRow(rowcount).CreateCell(colcount).CellStyle = SacofooterStyle;
                if (d.HoursWorked.HasValue && (decimal)d.HoursWorked.Value > 0)
                    sheet.GetRow(rowcount).GetCell(colcount).SetCellValue(((decimal)d.HoursWorked.Value).ToString("#0.00"));
                else
                    sheet.GetRow(rowcount).GetCell(colcount).SetCellValue(string.Empty);
                colcount++;
            }
            sheet.GetRow(rowcount).CreateCell(colcount).CellStyle = SacofooterStyle;
            sheet.GetRow(rowcount).GetCell(colcount).SetCellValue(((decimal)rp.DaySacoTotals.Sum(t => t.HoursWorked)).ToString("#0.00"));



            MemoryStream ms = new MemoryStream();

            // Writing the workbook content to the FileStream...
            wb.Write(ms);

            // Sending the server processed data back to the user computer...
            return File(ms.ToArray(), "application/vnd.ms-excel", coll["ddlUsers"]+"_"+coll["txtDateFrom"]+"_"+coll["txtDateTo"]+"_TimeSheetReport.xls");
        }

        private void GeneratePDF(ReportPackage myReportPackage)
        {

            if (myReportPackage.TimeSheets.Count > 0)
            {
                #region Table Header Collection
                List<string> ColList = new List<string>();
                ColList.Add("Prj No.");
                ColList.Add("SAP");
                ColList.Add("Cost Code");
                ColList.Add("Project Name");
                for (DateTime dt = myReportPackage.DateFrom; dt < myReportPackage.DateTo.AddDays(1); dt = dt.AddDays(1))
                {
                    ColList.Add(dt.ToString("ddd\ndd\nMMM"));
                }
                ColList.Add("Total");
                #endregion

                #region Detail Collection
                List<ReportRow> rows = new List<ReportRow>();
                int colcount = 0;
                int rowcount = 1;    
                foreach (var r in myReportPackage.TimeSheets)
                {
                    colcount = 0;
                    List<ReportCell> cells = new List<ReportCell>();
                    cells.Add(new ReportCell() { colnum = colcount, rownum = rowcount, colspan = 1, type = CellType.String, value = r.Project_Number.ToString() });
                    cells.Add(new ReportCell() { colnum = colcount++, rownum = rowcount, colspan = 1, type = CellType.String, value = r.SAP });
                    cells.Add(new ReportCell() { colnum = colcount++, rownum = rowcount, colspan = 1, type = CellType.String, value = r.WBS });
                    cells.Add(new ReportCell() { colnum = colcount++, rownum = rowcount, colspan = 1, type = CellType.String, value = r.Project_Name });
                    
                    foreach (var d in r.Days)
                    {
                        cells.Add(new ReportCell(){
                            colnum = colcount++,
                            rownum = rowcount,
                            colspan = 1,
                            type = CellType.Number,                            
                            value = (!d.HoursWorked.HasValue) ? string.Empty : d.HoursWorked.Value.ToString("#0.00")});
                        
                    }
                    cells.Add(new ReportCell() { colnum = colcount++, rownum = rowcount, colspan = 1, type = CellType.Number, value = r.Total.ToString() });

                    rows.Add(new ReportRow() { row = cells });
                    rowcount++;
                }
                #endregion

                #region Footer Collection
                List<string> FooterList = new List<string>();
                FooterList.Add("");
                FooterList.Add("");
                FooterList.Add("");
                FooterList.Add("");
                foreach (var d in myReportPackage.DayTotals)
                {
                    FooterList.Add((String.IsNullOrEmpty(d.HoursWorked.ToString())) ? string.Empty : (d.HoursWorked.Value == 0) ? string.Empty : d.HoursWorked.Value.ToString("#0.00"));
                }
                FooterList.Add(myReportPackage.GrandTotal.ToString());
                #endregion

                #region Saco Collection
                List<string> SacoList = new List<string>();
                SacoList.Add("");
                SacoList.Add("");
                SacoList.Add("");
                SacoList.Add("");
                foreach (var d in myReportPackage.DaySacoTotals)
                {
                    SacoList.Add((String.IsNullOrEmpty(d.HoursWorked.ToString())) ? string.Empty : (d.HoursWorked.Value == 0) ? string.Empty : d.HoursWorked.Value.ToString("#0.00"));
                }
                decimal sacototal = myReportPackage.DaySacoTotals.Sum(s => (decimal)s.HoursWorked);
                SacoList.Add(sacototal.ToString("#0.00"));
                #endregion

                #region Set the Column Widths
                float[] HeaderWidths = new float[colcount+1];
                HeaderWidths[0] = 26f;
                HeaderWidths[1] = 30f;
                HeaderWidths[2] = 30f;
                HeaderWidths[3] = 60f;
                for (int col = 4; col < colcount; col++)
                {
                    HeaderWidths[col] = 20f;
                }
                HeaderWidths[colcount] = 22f;
                #endregion


                //Create New PDF Timesheet Report
                IPDFReport myReport = new TimeSheetReport(true);

                //Assgin Column, Row and Footer collections to MyReport Object
                myReport.ColList = ColList;
                myReport.FooterList = FooterList;
                myReport.ReportRows = rows;
                myReport.SacoList = SacoList;
                
                //Set Report Properties
                myReport.Start = myReportPackage.DateFrom;
                myReport.End = myReportPackage.DateTo;
                myReport.filename = "TSD_TimeSheets_" + myReportPackage.User_CAI + "_" + myReportPackage.DateFrom.ToString("dd-MMM-yyyy") + "_" + myReportPackage.DateTo.ToString("dd-MMM-yyyy");
                myReport.PageHeight = 595;
                myReport.LogoImagePath = "~/images/img_hallmark.gif";
                myReport.ReportDescription = "TimeSheet";

                //Assign Column Widths to myReport
                myReport.Headerwidths = HeaderWidths;

                //Write myReport
                myReport.GenerateXMLReport();

                //Open Report and browswer
                myReport.OpenPDF();
            }

        }

        private void GenerateIlithaPDF(ReportPackage myReportPackage)
        {

            if (myReportPackage.TimeSheets.Count > 0)
            {
                #region Table Header Collection
                List<string> ColList = new List<string>();
                ColList.Add("MOC");
                ColList.Add("SAP#");
                ColList.Add("Cost Code");
                ColList.Add("Project");
                for (DateTime dt = myReportPackage.DateFrom; dt < myReportPackage.DateTo.AddDays(1); dt = dt.AddDays(1))
                {
                    ColList.Add(dt.ToString("ddd\ndd\nMMM"));
                }
                ColList.Add("Mth\nTotal");
                ColList.Add("Total");
                ColList.Add("Prj\nHrs");
                #endregion

                #region Detail Collection
                List<ReportRow> rows = new List<ReportRow>();
                int colcount = 0;
                int rowcount = 1;
                foreach (var r in myReportPackage.TimeSheets)
                {
                    colcount = 0;
                    List<ReportCell> cells = new List<ReportCell>();
                    cells.Add(new ReportCell() { colnum = colcount, rownum = rowcount, colspan = 1, type = CellType.String, value = r.Project_Number.ToString() });
                    cells.Add(new ReportCell() { colnum = colcount++, rownum = rowcount, colspan = 1, type = CellType.String, value = r.SAP });
                    cells.Add(new ReportCell() { colnum = colcount++, rownum = rowcount, colspan = 1, type = CellType.String, value = r.WBS });
                    cells.Add(new ReportCell() { colnum = colcount++, rownum = rowcount, colspan = 1, type = CellType.String, value = r.Project_Name });

                    foreach (var d in r.Days)
                    {
                        cells.Add(new ReportCell()
                        {
                            colnum = colcount++,
                            rownum = rowcount,
                            colspan = 1,
                            type = CellType.Number,
                            value = (!d.HoursWorked.HasValue) ? string.Empty : d.HoursWorked.Value.ToString("#0.00")
                        });

                    }
                    cells.Add(new ReportCell() { colnum = colcount++, rownum = rowcount, colspan = 1, type = CellType.Number, value = r.MonthTotal.ToString("#0.00") });
                    cells.Add(new ReportCell() { colnum = colcount++, rownum = rowcount, colspan = 1, type = CellType.Number, value = (r.SAP != "Leave")? r.Total.ToString("#0.00"): string.Empty });
                    cells.Add(new ReportCell() { colnum = colcount++, rownum = rowcount, colspan = 1, type = CellType.Number, value = (r.ProjectHours == 0 || r.SAP == "Leave") ? string.Empty : r.ProjectHours.ToString("#0.00") });

                    rows.Add(new ReportRow() { row = cells });
                    rowcount++;
                }
                #endregion

                #region Footer Collection
                List<string> FooterList = new List<string>();
                FooterList.Add("");
                FooterList.Add("");
                FooterList.Add("");
                FooterList.Add("");
                foreach (var d in myReportPackage.DayTotals)
                {
                    FooterList.Add((String.IsNullOrEmpty(d.HoursWorked.ToString())) ? string.Empty : (d.HoursWorked.Value == 0) ? string.Empty : d.HoursWorked.Value.ToString("#0.00"));
                }
                FooterList.Add(myReportPackage.GrandTotal.ToString());
                FooterList.Add("");
                FooterList.Add("");

                #endregion

                #region Saco Collection
                List<string> SacoList = new List<string>();
                SacoList.Add("");
                SacoList.Add("");
                SacoList.Add("");
                SacoList.Add("");
                foreach (var d in myReportPackage.DaySacoTotals)
                {
                    SacoList.Add((String.IsNullOrEmpty(d.HoursWorked.ToString())) ? string.Empty : (d.HoursWorked.Value == 0) ? string.Empty : d.HoursWorked.Value.ToString("#0.00"));
                }
                decimal sacototal = myReportPackage.DaySacoTotals.Sum(s => (decimal)s.HoursWorked);
                SacoList.Add(sacototal.ToString("#0.00"));
                SacoList.Add("");
                SacoList.Add("");

                #endregion



                #region Set the Column Widths
                float[] HeaderWidths = new float[colcount+1];
                HeaderWidths[0] = 26f;
                HeaderWidths[1] = 30f;
                HeaderWidths[2] = 30f;
                HeaderWidths[3] = 60f;
                for (int col = 4; col < colcount-2; col++)
                {
                    HeaderWidths[col] = 19f;
                }
                HeaderWidths[colcount-2] = 24f;
                HeaderWidths[colcount-1] = 24f;
                HeaderWidths[colcount] = 24f; //the last column
                #endregion


                //Create New PDF Timesheet Report
                IPDFReport myReport = new IlithaTimeSheetReport(true);

                //Assgin Column, Row and Footer collections to MyReport Object
                myReport.ColList = ColList;
                myReport.FooterList = FooterList;
                myReport.ReportRows = rows;
                myReport.SacoList = SacoList;

                //Set Report Properties
                myReport.Start = myReportPackage.DateFrom;
                myReport.End = myReportPackage.DateTo;
                myReport.User = myReportPackage.User;
                myReport.filename = "TSD_TimeSheets_" + myReportPackage.User_CAI + "_" + myReportPackage.DateFrom.ToString("dd-MMM-yyyy") + "_" + myReportPackage.DateTo.ToString("dd-MMM-yyyy");
                myReport.PageHeight = 595;
                myReport.LogoImagePath = "~/images/img_hallmark.gif";
                myReport.ReportDescription = "TimeSheet";

                //Assign Column Widths to myReport
                myReport.Headerwidths = HeaderWidths;

                //Write myReport
                myReport.GenerateXMLReport();

                //Open Report and browswer
                myReport.OpenPDF();
            }

        }



        private int MonthDays(DateTime myDate)
        {
            DateTime firstDayOfThisMonth = myDate.Subtract(TimeSpan.FromDays(myDate.Day - 1));
            DateTime firstDayOfNextMonth = firstDayOfThisMonth.AddMonths(1);
            DateTime lastDayOfThisMonth = firstDayOfNextMonth.Subtract(TimeSpan.FromDays(1));
            return lastDayOfThisMonth.Day - firstDayOfThisMonth.Day + 1;
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Report(FormCollection coll)
        {
            DateTime myDate = DateTime.Now;

            DateTime firstDayOfThisMonth = DateTime.Parse(coll["txtDateFrom"]);
            DateTime lastDayOfThisMonth = DateTime.Parse(coll["txtDateTo"]);

            ViewData["Users"] = new SelectList(userdb.GetUserItems(), "cai", "detail", coll["ddlUsers"]);
            ViewData["MonthDays"] = lastDayOfThisMonth.Subtract(firstDayOfThisMonth).Days + 1;
            return View("Index", timedb.GetReportByUser(coll["ddlUsers"], firstDayOfThisMonth, lastDayOfThisMonth));

        }


        [AcceptVerbs("GET", "POST")]
        public ActionResult ExcelReport(string dateFrom, string dateTo, string User_CAI)
        {
            DateTime firstDay = DateTime.Parse(dateFrom);
            DateTime lastDay = DateTime.Parse(dateTo);

            return View(timedb.GetReportByUser(User_CAI, firstDay, lastDay));
        }

        private string DownloadReportHTMLContent(string dateFrom, string dateTo, string User_CAI)
        {

            string linkFormat = Url.Action("ExcelReport", "Report", null, "http");
            linkFormat += "?dateFrom={0}&dateTo={1}&User_CAI={2}";
            System.Net.WebClient wc = new System.Net.WebClient();
            return wc.DownloadString(string.Format(linkFormat, dateFrom, dateTo, User_CAI));
        }

    }
}
