using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TSDTReports.Models
{
    public class TimeSheetRepository : ITimesheetRepository 
    {
        tsdtDataContext db = new tsdtDataContext();
        IResourceRepository rdb = new ResourceRepository();
        IUserRepository userdb = new UserRepository();
        ISacoRepository sacodb = new SacoRepository();
        public void Add(tbl_TimeSheet ts)
        {
            db.tbl_TimeSheets.InsertOnSubmit(ts);
        }

        public void Save()
        {
            db.SubmitChanges();
        }

        private bool ProjectExists(int ProjectID)
        {
            return db.tbl_Projects.Where(i => i.Project_ID == ProjectID).Count() > 0;
        }

        public TimeSheetPackage GetTimeSheetByUser(string User_CAI, DateTime myDate)
        {
            DateTime firstday = new DateTime(myDate.Year, myDate.Month, 1, 0, 0, 0);
            DateTime lastDay = firstday.AddMonths(1).Subtract(TimeSpan.FromDays(1));
            lastDay = new DateTime(lastDay.Year, lastDay.Month, lastDay.Day, 23, 59, 59);

            TimeSheetPackage myPackage = GetTimeSheet(User_CAI, firstday, lastDay);
            myPackage.Date = myDate;
            myPackage.User_CAI = User_CAI;
            return myPackage;
        }

        private TimeSheetPackage GetTimeSheet(string User_CAI, DateTime firstDayOfThisMonth, DateTime lastDayOfThisMonth)
        {
            tbl_User myUser = userdb.GetUserByID(User_CAI);

            if (myUser.EmployeeNumber == "")
                myUser.EmployeeNumber = "temp";

            TimeSheetPackage myPackage = new TimeSheetPackage();

            myPackage.GrandTotal = 0;
            myPackage.DayTotals = new List<day>();
            myPackage.DaySacoTotals = sacodb.SacoDays(myUser.EmployeeNumber, firstDayOfThisMonth, lastDayOfThisMonth);
            myPackage.TimeSheets = new List<timesheet>();
            for (DateTime d = firstDayOfThisMonth; d < lastDayOfThisMonth; d= d.AddDays(1))
            {

                myPackage.DayTotals.Add(new day() { Month = d.Month, Year = d.Year, Day = d.Day, HoursWorked = 0M });

            }


            IQueryable<tbl_Resource> myResources = rdb.FindResourcesByCAI(User_CAI).OrderBy(t => t.tbl_Project.Project_Number);
            if (myResources.Count() > 0)
            {
                foreach (var res in myResources)
                {
                    if (ProjectExists((int)res.Project_ID))
                    {
                        timesheet resourceTimeSheet = new timesheet();

                        resourceTimeSheet.editStatus = true;

                        if (res.tbl_Project.tbl_Status.Status == "CLSD" ||
                            res.tbl_Project.tbl_Status.Status == "W" || 
                            res.tbl_Project.tbl_Status.Status == "TC"                           
                            )
                            resourceTimeSheet.editStatus = false;
                        if (res.active == false)
                            resourceTimeSheet.editStatus = false;

                        resourceTimeSheet.Resource_ID = res.Resource_ID;
                        resourceTimeSheet.Project_ID = (int)res.Project_ID;
                        resourceTimeSheet.Project_Number = (int)res.tbl_Project.Project_Number;
                        resourceTimeSheet.Project_Name = res.tbl_Project.Project_Name;
                        resourceTimeSheet.Project_Name_Abrv = res.tbl_Project.Project_Name;
                        resourceTimeSheet.SAP = res.tbl_Project.SAP_Number;
                        resourceTimeSheet.WBS = res.tbl_Project.WBS;
                        resourceTimeSheet.Status = res.tbl_Project.tbl_Status.Status;
                        resourceTimeSheet.hide = res.Hide;
                        resourceTimeSheet.ProjectHours = (!res.ResourceHours.HasValue) ? 0M: res.ResourceHours.Value;
                        decimal? test = this.GetHoursByResourceAndDate(res.Resource_ID, DateTime.Parse("01-Jan-2007"), lastDayOfThisMonth).Sum(s => s.Hours);
                        resourceTimeSheet.Total = (!test.HasValue) ? 0M : test.Value;
                        

                        IQueryable<tbl_TimeSheet> timeList = this.GetHoursByResourceAndDate(
                            res.Resource_ID,
                            DateTime.Parse(String.Format("{0: yyyy/MM/dd 00:00:00}", firstDayOfThisMonth)),
                            DateTime.Parse(String.Format("{0: yyyy/MM/dd 23:59:59}", lastDayOfThisMonth))).OrderBy(t => t.Date);
                        decimal Monthtotal = 0M;

                        resourceTimeSheet.Days = new List<day>();
                        //Initialize Days to null values
                        int count = 1;
                        for (DateTime d = firstDayOfThisMonth; d < lastDayOfThisMonth; d.AddDays(1))
                        {

                            resourceTimeSheet.Days.Add(new day() { Day = count, HoursWorked = null });
                            d = d.AddDays(1);
                            count++;
                        }

                        if (timeList.Count() > 0)
                        {
                            foreach (var t in timeList)
                            {
                                foreach (var d in resourceTimeSheet.Days)
                                {
                                    if (((DateTime)t.Date).Day == d.Day)
                                    {
                                       
                                        d.HoursWorked = (decimal)t.Hours;
                                        
                                        Monthtotal += (decimal)t.Hours;
                                    }
                                }


                            }
                        }


                        resourceTimeSheet.MonthTotal = Monthtotal;
                        if (resourceTimeSheet.SAP != "Leave")
                        myPackage.GrandTotal += Monthtotal;
                        for (int j = 0; j < resourceTimeSheet.Days.Count; j++)
                        {
                            if (resourceTimeSheet.Days[j].HoursWorked.HasValue)
                            {
                                if (resourceTimeSheet.SAP != "Leave")
                                myPackage.DayTotals[j].HoursWorked += resourceTimeSheet.Days[j].HoursWorked.Value;
                            }
                           
                        }
                        if (resourceTimeSheet.MonthTotal > 0 || resourceTimeSheet.hide == false)
                        myPackage.TimeSheets.Add(resourceTimeSheet);

                    }
                }

            }
            return myPackage;
        }

        public IQueryable<tbl_TimeSheet> GetHoursByResourceAndDate(int resource_id, DateTime FirstDay, DateTime LastDay)
        {
            var test =  db.tbl_TimeSheets.Where(t => t.Resource_ID == resource_id && t.Date >= FirstDay && t.Date < LastDay);
            return test;
        }

        public void DeleteTimesheet(int resource_id, DateTime myDate)
        {
            DateTime firstDayOfThisMonth = myDate.Subtract(TimeSpan.FromDays(myDate.Day - 1));
            DateTime firstDayOfNextMonth = firstDayOfThisMonth.AddMonths(1);
            DateTime lastDayOfThisMonth = firstDayOfNextMonth.Subtract(TimeSpan.FromDays(1));

            var q = from ts in db.tbl_TimeSheets
                    where (ts.Resource_ID == resource_id &&
                    ts.Date >= DateTime.Parse(String.Format("{0: yyyy/MM/dd 00:00:00}", firstDayOfThisMonth)) &&
                    ts.Date < DateTime.Parse(String.Format("{0: yyyy/MM/dd 23:59:59}", lastDayOfThisMonth)))
                    select ts;
            db.tbl_TimeSheets.DeleteAllOnSubmit(q);
        }

        public void AddTimeSheetEntriesForDays(List<day> days, int Resource_id)
        {
            foreach (var h in days)
            {
                tbl_TimeSheet mytime = new tbl_TimeSheet();
                mytime.Resource_ID = Resource_id;
                mytime.Date = new DateTime(h.Year, h.Month, h.Day);
                mytime.Hours = (decimal)h.HoursWorked;
                db.tbl_TimeSheets.InsertOnSubmit(mytime);
            }
        }

        public TimeSheetPackage GetTimeSheetByUser(string User_CAI, DateTime myDateFrom, DateTime myDateTo)
        {
            TimeSheetPackage myPackage = GetTimeSheet(User_CAI, myDateFrom, myDateTo);
            myPackage.Date = myDateFrom;
            myPackage.User_CAI = User_CAI;
            return myPackage;
        }

        public ReportPackage GetReportByUser(string User_CAI, DateTime myDateFrom, DateTime myDateTo)
        {
            //The Report Package is a container for all data that must be displayed on the Report View
            //The Report Package is used as the Model for the report index View
            //The Report Package contains the following: 
            //  1. user information in the form of a user object
            //  2. collection of timesheets for active(unhidden) projects for the date period. 
            //    The timesheet object contains project information and a collection of days.
            ReportPackage myPackage = new ReportPackage();
            

            tbl_User myuser = userdb.GetUserByID(User_CAI);
            myPackage.User = myuser;
            myPackage.DateFrom = myDateFrom;
            myPackage.DateTo = myDateTo;
            myPackage.User_CAI = User_CAI;
            myPackage.GrandTotal = 0;
            myPackage.DayTotals = new List<day>();
            myPackage.DaySacoTotals = sacodb.SacoDays(myuser.EmployeeNumber, myDateFrom, myDateTo);

            myPackage.TimeSheets = new List<timesheet>();
            int i = 1;
            for (DateTime d = myDateFrom; d < myDateTo.AddDays(1); d= d.AddDays(1))
            {

                myPackage.DayTotals.Add(new day() {Number = i, Day = i, HoursWorked = 0M });
                i++;
            }
            IQueryable<tbl_Resource> myResources = rdb.FindResourcesByCAI(User_CAI).OrderBy(t => t.tbl_Project.Project_Number);
            if (myResources.Count() > 0)
            {
                foreach (var res in myResources)
                {
                    if (ProjectExists((int)res.Project_ID))
                    {
                        timesheet resourceTimeSheet = new timesheet();

                        resourceTimeSheet.editStatus = true;

                        if (res.tbl_Project.tbl_Status.Status == "CLSD")
                            resourceTimeSheet.editStatus = false;
                        if (res.active == false)
                            resourceTimeSheet.editStatus = false;

                        resourceTimeSheet.Resource_ID = res.Resource_ID;
                        resourceTimeSheet.Project_ID = (int)res.Project_ID;
                        resourceTimeSheet.Project_Number = (int)res.tbl_Project.Project_Number;
                        resourceTimeSheet.Project_Name = res.tbl_Project.Project_Name;
                        resourceTimeSheet.Project_Name_Abrv = res.tbl_Project.Project_Name;
                        resourceTimeSheet.SAP = res.tbl_Project.SAP_Number;
                        resourceTimeSheet.WBS = res.tbl_Project.WBS;
                        resourceTimeSheet.Status = res.tbl_Project.tbl_Status.Status;
                        resourceTimeSheet.hide = res.Hide;
                        resourceTimeSheet.ProjectHours = (!res.ResourceHours.HasValue) ? 0M : res.ResourceHours.Value;
                        decimal? test = this.GetHoursByResourceAndDate(res.Resource_ID, DateTime.Parse("01-Jan-2007"), DateTime.Now).Sum(s => s.Hours);
                        resourceTimeSheet.Total = (!test.HasValue) ? 0M : test.Value;
                        

                        IQueryable<tbl_TimeSheet> timeList = this.GetHoursByResourceAndDate(
                            res.Resource_ID,
                            DateTime.Parse(String.Format("{0: yyyy/MM/dd 00:00:00}", myDateFrom)),
                            DateTime.Parse(String.Format("{0: yyyy/MM/dd 23:59:59}", myDateTo))).OrderBy(t => t.Date);
                        decimal Monthtotal = 0M;

                        resourceTimeSheet.Days = new List<day>();
                        //Initialize Days to null values

                        for (DateTime d = myDateFrom; d < myDateTo.AddDays(1); d = d.AddDays(1))
                        {
                            resourceTimeSheet.Days.Add(new day() { Day = d.Day, Month = d.Month, Year=d.Year , HoursWorked = null });
                        }

                        if (timeList.Count() > 0)
                        {
                            foreach (var t in timeList)
                            {
                                foreach (var d in resourceTimeSheet.Days)
                                {
                                    if (((DateTime)t.Date).Day == d.Day && ((DateTime)t.Date).Month == d.Month && ((DateTime)t.Date).Year == d.Year)
                                    {
                                        
                                        d.HoursWorked = (decimal)t.Hours;
                                        Monthtotal += (decimal)t.Hours;
                                    }
                                }


                            }
                        }


                        resourceTimeSheet.MonthTotal = Monthtotal;
                        if (resourceTimeSheet.SAP != "Leave")
                        myPackage.GrandTotal += Monthtotal;
                        for (int j = 0; j < resourceTimeSheet.Days.Count; j++)
                        {
                            if (resourceTimeSheet.Days[j].HoursWorked.HasValue)
                            {
                                if (resourceTimeSheet.SAP != "Leave")
                                myPackage.DayTotals[j].HoursWorked += resourceTimeSheet.Days[j].HoursWorked.Value;
                            }
                        }
                        if (Monthtotal > 0 || resourceTimeSheet.hide == false)
                        myPackage.TimeSheets.Add(resourceTimeSheet);

                    }
                }

            }

            return myPackage;
        }

    }
}
