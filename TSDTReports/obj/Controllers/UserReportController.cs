using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using TSDTReports.Models;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace TSDTReports.Controllers
{
    public class UserReportController : Controller
    {
        IUserRepository userdb = new UserRepository();
        IProjectRepository projectdb = new ProjectRepository();

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Index(FormCollection collection, string submitbutton)
        {            
            //Title'
            string testhtml = Request.Form["table"];
            ViewData["Title"] = "User Report";
            ViewData["Users"] = new SelectList(userdb.GetUserItems(), "cai", "detail", collection["ddlusers"]);

            ViewData["startdate"] = collection["txtStartDate"];
            ViewData["enddate"] = collection["txtEndDate"]; 
            string userid = collection["ddlUsers"];
            tbl_User myUser = userdb.GetUserByID(userid);
            ViewData["User"] = myUser;
            var test = projectdb.GetProjectHours(collection["ddlUsers"], DateTime.Parse(collection["txtStartDate"]), DateTime.Parse(collection["txtEndDate"]));
            ViewData["ProjectHours"] = test;
            switch (submitbutton)
            {
                case "Print":
                    if (test.Count() > 0)
                    {
                        List<string> ColList = new List<string>();
                        ColList.Add("Number");
                        ColList.Add("Name");
                        ColList.Add("Staus");
                        ColList.Add("Hours");

                        List<ReportRow> rows = new List<ReportRow>();
                        decimal totalhours = 0M;
                        int i = 0;

                        foreach (var item in test)
                        {
                            List<ReportCell> cells = new List<ReportCell>();
                            cells.Add(new ReportCell() { colnum = 0, colspan = 1, rownum = i, value = item.ProjectNumber.ToString(), type = CellType.String });
                            cells.Add(new ReportCell() { colnum = 1, colspan = 1, rownum = i, value = item.ProjectName, type = CellType.String });
                            cells.Add(new ReportCell() { colnum = 2, colspan = 1, rownum = i, value = item.ProjectStatus, type = CellType.String });
                            cells.Add(new ReportCell() { colnum = 3, colspan = 1, rownum = i, value = item.Hours.ToString(), type = CellType.Number });
                            i++;
                            totalhours += (decimal)item.Hours;
                            rows.Add(new ReportRow() { row = cells });

                        }

                        IPDFReport myReport = new Report(false);
                        float[] HeaderWidths = { 10, 70, 10, 10 };
                        myReport.ColList = ColList;
                        myReport.ReportRows = rows;
                        myReport.total = totalhours.ToString("#,##0.00");
                        myReport.Start = DateTime.Parse(collection["txtStartDate"]);
                        myReport.End = DateTime.Parse(collection["txtEndDate"]);
                        myReport.Name = myUser.User_FirstName + " " + myUser.User_LastName + " [" + myUser.User_CAI + "]";
                        myReport.Headerwidths = HeaderWidths;
                        myReport.filename = "TSD_User_Report_" + myUser.User_FirstName + "_" + myUser.User_LastName + "_[" + myUser.User_CAI + "]";
                        myReport.PageHeight = 595;
                        myReport.LogoImagePath = "~/images/img_hallmark.gif";
                        myReport.myXML = testhtml;
                        myReport.query = "User Report: " + myUser.User_FirstName + " " + myUser.User_LastName + " [" + myUser.User_CAI + "]"; ;
                        myReport.ReportDescription = "User Report";
                        myReport.GenerateXMLReport();
                        myReport.OpenPDF();
                    }


                    break;
                case "Get User Report":
                    break;
            }



            return View();
        }

        public ActionResult Index()
        {
            ViewData["Users"] = new SelectList(userdb.GetUserItems(), "cai", "detail");
            ViewData["Title"] = "User Report";
            return View();
        }
    }
}
