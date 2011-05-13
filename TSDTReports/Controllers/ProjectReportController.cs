using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Collections;
using TSDTReports.Models;
using System.Text;

namespace TSDTReports.Controllers
{



    public class ProjectReportController : Controller
    {

        IProjectRepository projectdb = new ProjectRepository();
        IUserRepository userdb = new UserRepository();

        //1. Search
        //2. Get Project Report
        //3. Print 
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Index(FormCollection collection, string submitbutton)
        {
            List<SelectListItem> collist = new List<SelectListItem>();
            List<SelectListItem> operlist = new List<SelectListItem>();
            
            collist.Add(new SelectListItem{ Text = "Project Number", Value = "Project_Number" });
            collist.Add(new SelectListItem { Text = "SAP", Value = "SAP_Number" });
            collist.Add(new SelectListItem { Text = "WBS", Value = "WBS" });
            collist.Add(new SelectListItem { Text = "Project Name", Value = "Project_Name" });
            
            operlist.Add(new SelectListItem{ Text = "begins with", Value="bw"});
            operlist.Add(new SelectListItem{ Text = "equals", Value="eq"});
            operlist.Add(new SelectListItem{ Text = "contains", Value="cn"});


            ViewData["Cols"] = new SelectList(collist, "Value", "Text", collection["ddlCols"] );
            ViewData["Oper"] = new SelectList(operlist, "Value", "Text", collection["ddlOper"]);
            
            ViewData["Title"] = "Project Report";
            ViewData["Search"] = collection["txtSearch"];
            ViewData["startdate"] = collection["txtStartDate"];
            ViewData["enddate"] = collection["txtEndDate"];

            try
            {
                ViewData["Projects"] = projectdb.GetSearchedProjects(collection["ddlCols"], collection["txtSearch"], collection["ddlOper"] ).ToList();
            }
            catch (Exception ex)
            {
                ViewData["Message"] = ex.Message;
            }

            switch (submitbutton)
            {
                case "Search":
                    SearchProjects(collection);
                    break;
                case "Get Project Report":
                    GetProjectReport(collection);
                    break;
                case "Print":
                    Print(collection);
                    break;


            }


            return View();
        }



        public ActionResult Print(FormCollection collection)
        {
            StringBuilder sb = new StringBuilder();

            //string userid = collection["ddlUsers"];
            //tbl_User myUser = db.GetUserByID(userid);

            foreach (var item in collection)
            {
                try
                {
                    if (item.ToString().Contains("chk"))    //Only check the checkboxes
                    {
                        ViewData[item.ToString()] = collection[item.ToString()].Replace(",false", "");
                        if (collection[item.ToString()] == "false")
                        { }//do nothing
                        else
                            sb.Append(item.ToString().Replace("chk", "") + ",");
                    }
                }
                catch
                {
                    //Do nothing
                }

            }

          

            string[] myIds = sb.ToString().Trim(',').Split(',');

            try
            {
                int test = int.Parse(myIds[0]);
            }
            catch
            {
                ViewData["Message"] = "Please select at least one project item";
                return View("Index");
            }

            try
            {
                var userdatacollection = userdb.GetUsersHoursForProject(myIds, DateTime.Parse(collection["txtStartDate"]), DateTime.Parse(collection["txtEndDate"]));
                ViewData["UserDataCollection"] = userdatacollection;
                if (userdatacollection.Count() > 0)
                {
                    List<string> ColList = new List<string>();
                    ColList.Add("Last Name");
                    ColList.Add("First Name");
                    ColList.Add("CAI");
                    ColList.Add("Hours");

                    decimal totalhours = 0M;
                    List<ReportRow> rows = new List<ReportRow>();
                    int i = 0;
                    foreach (var item in userdatacollection)
                    {
                        List<ReportCell> cells = new List<ReportCell>();
                        cells.Add(new ReportCell() { colnum = 0, colspan=1 , rownum = i, value = item.LastName, type  = CellType.String});
                        cells.Add(new ReportCell() { colnum = 1, colspan=1 , rownum = i, value = item.FirstName, type = CellType.String});
                        cells.Add(new ReportCell() { colnum = 2, colspan=1 , rownum = i, value = item.CAI, type= CellType.String });
                        cells.Add(new ReportCell() { colnum = 3, colspan=1 , rownum = i, value = item.hours.ToString(), type= CellType.Number});
                        i++;
                        totalhours += (decimal)item.hours;
                        rows.Add(new ReportRow() { row = cells }); 

                    }

                    IPDFReport myReport = new ProjectReport(false);
                    float[] HeaderWidths = { 40, 40, 10, 10 };
                    myReport.ColList = ColList;
                    myReport.ReportRows = rows;
                    myReport.total = totalhours.ToString("#,##0.00");
                    myReport.Start = DateTime.Parse(collection["txtStartDate"]);
                    myReport.End = DateTime.Parse(collection["txtEndDate"]);
                    myReport.ProjectList = projectdb.GetProjectsByIds(myIds);
                    myReport.Headerwidths = HeaderWidths;
                    myReport.filename = "TSD_Project_Report";// +myUser.User_FirstName + "_" + myUser.User_LastName + "_[" + myUser.User_CAI + "]";
                    myReport.PageHeight = 595;
                    myReport.LogoImagePath = "~/images/img_hallmark.gif";
                    //myReport.myXML = testhtml;
                    myReport.query = "Project Report: ";// +myUser.User_FirstName + " " + myUser.User_LastName + " [" + myUser.User_CAI + "]"; ;
                    myReport.ReportDescription = "User Report";
                    myReport.GenerateXMLReport();
                    myReport.OpenPDF();
                }
                return View("Index");
            }
            catch (Exception ex)
            {
                string error = "";
                if (ex.Message.Contains("DateTime"))
                    error = "Invalid Date Value";
                else
                    error = ex.Message;
                ViewData["Message"] = error;
            }
            return View("Index");
        }

        public ActionResult SearchProjects(FormCollection collection)
        {

            StringBuilder sb = new StringBuilder();
            foreach (var item in collection)
            {
                try
                {
                    if (item.ToString().Contains("chk"))    //Only check the checkboxes
                    {
                        ViewData[item.ToString()] = collection[item.ToString()].Replace(",false", "");
                        if (collection[item.ToString()] == "false")
                        { }//do nothing
                        else
                            sb.Append(item.ToString().Replace("chk", "") + ",");
                    }
                }
                catch
                {
                    //Do nothing
                }

            }


            string[] myIds = sb.ToString().Trim(',').Split(',');

            try
            {
                int test = int.Parse(myIds[0]);
            }
            catch
            {
                ViewData["Message"] = "Please select at least one project item";
                return View("Index");
            }

            return View("Index");
        }

        public ActionResult GetProjectReport(FormCollection collection)
        {

            StringBuilder sb = new StringBuilder();
            foreach (var item in collection)
            {
                try
                {
                    if (item.ToString().Contains("chk"))    //Only check the checkboxes
                    {
                        ViewData[item.ToString()] = collection[item.ToString()].Replace(",false", "");
                        if (collection[item.ToString()] == "false")
                        { }//do nothing
                        else
                            sb.Append(item.ToString().Replace("chk", "") + ",");
                    }
                }
                catch
                {
                    //Do nothing
                }

            }


            string[] myIds = sb.ToString().Trim(',').Split(',');

            try
            {
                int test = int.Parse(myIds[0]);
            }
            catch
            {
                 ViewData["Message"] = "Please select at least one project item";
                 return View("Index");
            }

            try
            {
                var userdatacollection = userdb.GetUsersHoursForProject(myIds, DateTime.Parse(collection["txtStartDate"]), DateTime.Parse(collection["txtEndDate"]));
                ViewData["UserDataCollection"] = userdatacollection;
                return View();
            }
            catch (Exception ex)
            {
                string error = "";
                if (ex.Message.Contains("DateTime"))
                    error = "Invalid Date Value";
                else
                    error = ex.Message;
                ViewData["Message"] = error;
            }
            return View("Index");
        }

        //Initial Get
        public ActionResult Index()
        {
            ViewData["Title"] = "Project Report";
            List<SelectListItem> collist = new List<SelectListItem>();
            List<SelectListItem> operlist = new List<SelectListItem>();

            collist.Add(new SelectListItem { Text = "Project Number", Value = "Project_Number" });
            collist.Add(new SelectListItem { Text = "SAP", Value = "SAP_Number" });
            collist.Add(new SelectListItem { Text = "WBS", Value = "WBS" });
            collist.Add(new SelectListItem { Text = "Project Name", Value = "Project_Name" });

            operlist.Add(new SelectListItem { Text = "begins with", Value = "bw" });
            operlist.Add(new SelectListItem { Text = "equals", Value = "eq" });
            operlist.Add(new SelectListItem { Text = "contains", Value = "cn" });


            ViewData["Cols"] = new SelectList(collist, "Value", "Text");
            ViewData["Oper"] = new SelectList(operlist, "Value", "Text");

            return View();
        }


    }
}
