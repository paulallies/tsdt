using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections;

namespace TSDTReports.Models
{

    public interface ITimesheetRepository
    {
        void Add(tbl_TimeSheet ts);
        void Save();
        TimeSheetPackage GetTimeSheetByUser(string User_CAI, DateTime myDate);
        ReportPackage GetReportByUser(string User_CAI, DateTime myDateFrom, DateTime myDateTo);
        IQueryable<tbl_TimeSheet> GetHoursByResourceAndDate(int resource_int, DateTime FirstDay, DateTime LastDay);
        void DeleteTimesheet(int resource_id, DateTime myDate);
        void AddTimeSheetEntriesForDays(List<day> days, int Resource_id);
    }

    public class day
    {
        public decimal? HoursWorked{ get; set; }
        public int Number { get; set; }
        public int Day { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
        public DateTime Date { get; set; }
    }


    public class timesheet 
    {
        public int Project_ID { get; set; }
        public string Project_Name { get; set; }
        public string Project_Name_Abrv { get; set; }
        public int Project_Number { get; set; }
        public int Resource_ID { get; set; }
        public string SAP { get; set; }
        public string WBS { get; set; }
        public string Status { get; set; }
        public decimal Total { get; set; }
        public List<day> Days { get; set; }
        public bool hide { get; set; }
        public bool active { get; set; }        
        public bool editStatus { get; set; }


    }

    public class TimeSheetPackage
    {
        public DateTime Date { get; set; }
        public decimal GrandTotal { get; set; }
        public List<timesheet> TimeSheets { get; set; }
        public string User_CAI { get; set; }
        public List<day> DayTotals { get; set; }
        public List<day> DaySacoTotals { get; set; }

    }

    public class ReportPackage
    {
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public decimal GrandTotal { get; set; }
        public List<timesheet> TimeSheets { get; set; }
        public string User_CAI { get; set; }
        public string User { get; set; }
        public List<day> DayTotals { get; set; }
        public List<day> DaySacoTotals { get; set; }
    }
}
