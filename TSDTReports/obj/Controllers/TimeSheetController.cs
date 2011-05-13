using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using TSDTReports.Models;

namespace TSDTReports.Controllers
{
    public class TimeSheetController : Controller
    {
        //
        // GET: /TimeSheet/
        IUserRepository userdb = new UserRepository();
        ITimesheetRepository timedb = new TimeSheetRepository();
        IRoleRepository roledb = new RoleRepository();
        IResourceRepository resourcedb = new ResourceRepository();

        public ActionResult Index(string ddlUsers, string txtDate)
        {

            string role = roledb.GetUserRolesByUser(User.Identity.Name.ToString().ToUpper().Replace("CT\\", ""));
            ViewData["Role"] = role;
            DateTime myDate = DateTime.Now;
            
            if(txtDate != null)
                myDate = DateTime.Parse(txtDate);            
            
            string User_CAI = "";
            if (ddlUsers == null)
                User_CAI = User.Identity.Name.ToUpper().Replace("CT\\", "");
               // User_CAI = "PALX";
            else
                User_CAI = ddlUsers;
            var users = new SelectList(userdb.GetUserItems(), "cai", "detail", User_CAI);
            var UserProjects = userdb.GetUserResources(User_CAI).ToList();
            var timesheets = timedb.GetTimeSheetByUser(User_CAI, myDate);

            ViewData["Users"] = users;
       //     ViewData["MonthDays"] = MonthDays(myDate);
            ViewData["UserProjects"] = UserProjects;
            return View(timesheets);
        }

        public ActionResult Edit(int id, DateTime myDate, string User_CAI)
        {
            //string User_CAI = "PALX";
            //string User_CAI = User.Identity.Name.ToUpper().Replace("CT\\", "");
            ViewData["Users"] = new SelectList(userdb.GetUserItems(), "cai", "detail", User_CAI);
            ViewData["Date"] = myDate.ToString("dd-MMM-yyyy");
            var ViewPackage = timedb.GetTimeSheetByUser(User_CAI, myDate);

            ViewData["MonthDays"] = MonthDays(myDate);
            ViewData["Resource_id"] = id;
            return View(ViewPackage);
        }

        private int MonthDays(DateTime myDate)
        {
            DateTime firstDayOfThisMonth = myDate.Subtract(TimeSpan.FromDays(myDate.Day - 1));
            DateTime firstDayOfNextMonth = firstDayOfThisMonth.AddMonths(1);
            DateTime lastDayOfThisMonth = firstDayOfNextMonth.Subtract(TimeSpan.FromDays(1));
            return lastDayOfThisMonth.Day - firstDayOfThisMonth.Day + 1;
        }

        public ActionResult Cancel(DateTime myDate, string User_CAI)
        {
            string role = roledb.GetUserRolesByUser(User.Identity.Name.ToString().ToUpper().Replace("CT\\", ""));
            ViewData["Role"] = role;
            //string User_CAI = "PALX";
            ViewData["Users"] = new SelectList(userdb.GetUserItems(), "cai", "detail", User_CAI);
            //ViewData["MonthDays"] = MonthDays(myDate);
            ViewData["UserProjects"] = userdb.GetUserResources(User_CAI).ToList();

            return View("Index",timedb.GetTimeSheetByUser(User_CAI, myDate));

        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Index(FormCollection coll)//, string date, string user,  int resid, string mode)
        {
            #region GetUser
            string User_CAI = "";
            if (coll["ddlUsers"] == null)
                User_CAI = User.Identity.Name.ToUpper().Replace("CT\\", "");
            // User_CAI = "PALX";
            else
                User_CAI = coll["ddlUsers"];
            #endregion

            #region GetDate
            DateTime myDate = DateTime.Now;
            if (coll["txtDate"] != null)
                myDate = DateTime.Parse(coll["txtDate"]);
            #endregion

            

            foreach (var item in coll)
            {
                try
                {
                    if (item.ToString().Contains("chk"))    //Only check the checkboxes
                    {
                        tbl_Resource myres = resourcedb.GetResource(int.Parse(item.ToString().Replace("chk", "")));
                        if (coll[item.ToString()].Replace(",false", "") == "true")
                        {
                            myres.Hide = true;
                        }
                        else if (coll[item.ToString()].Replace(",false", "") == "false")
                        {
                            myres.Hide = false;
                        }

                    }
                }
                catch
                {
                    //Do nothing
                }

            }

            try {
                resourcedb.Save();
            }
            catch { }
            string role = roledb.GetUserRolesByUser(User.Identity.Name.ToString().ToUpper().Replace("CT\\", ""));
            ViewData["Role"] = role;
            ViewData["Users"] = new SelectList(userdb.GetUserItems(), "cai", "detail", User_CAI);
            ViewData["UserProjects"] = userdb.GetUserResources(User_CAI).ToList();
            return View(timedb.GetTimeSheetByUser(User_CAI, myDate));
        }

        private void Update(int resource_id, DateTime myDate, FormCollection coll)
        {
            timedb.DeleteTimesheet(resource_id, myDate);
            List<day> myEntries = new List<day>();
           

            foreach (var item in coll)
            {
                if(item.ToString().Contains("r"))
                {
                    if(coll[item.ToString()] != string.Empty)

                    myEntries.Add(new day { Day = int.Parse(item.ToString().Replace("r", "")), Year = myDate.Year, Month = myDate.Month, HoursWorked = decimal.Parse(coll[item.ToString()].ToString()) });
                }
            }
            foreach (var h in myEntries)
            {
                tbl_TimeSheet mytime = new tbl_TimeSheet();
                mytime.Resource_ID = resource_id;
                mytime.Date = new DateTime(h.Year, h.Month, h.Day);
                mytime.Hours = (decimal)h.HoursWorked;
                timedb.Add(mytime);
            }
            timedb.Save();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Update(FormCollection coll)
        {
            string role = roledb.GetUserRolesByUser(User.Identity.Name.ToString().ToUpper().Replace("CT\\", ""));
            ViewData["Role"] = role;
            int resource_id = int.Parse(coll["resid"]);
            DateTime myDate = DateTime.Parse(coll["txtDate"]);
            timedb.DeleteTimesheet(resource_id, myDate); //Delete Old Timesheet entries for this resource for this date peroid

            List<day> myEntries = new List<day>(); //Find all non null Timetable hours for posted
            
            foreach (var item in coll)
            {
                if (item.ToString().Contains("dayentry"))
                {
                    if (coll[item.ToString()].Trim() != string.Empty)
                        myEntries.Add(new day { Day = int.Parse(item.ToString().Replace("dayentry", "")), Year = myDate.Year, Month = myDate.Month, HoursWorked = decimal.Parse(coll[item.ToString()].ToString().Trim()) });
                }
            }

            timedb.AddTimeSheetEntriesForDays(myEntries, resource_id); //Add Time entries for this resource
            timedb.Save();//Submit to database

            ViewData["Users"] = new SelectList(userdb.GetUserItems(), "cai", "detail", coll["ddlUsers"]);
            ViewData["MonthDays"] = MonthDays(myDate);
            ViewData["UserProjects"] = userdb.GetUserResources(coll["ddlUsers"]).ToList();
            return View("Index", timedb.GetTimeSheetByUser(coll["ddlUsers"], DateTime.Parse(coll["txtDate"])));
        }

    }
}
