using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TSDTReports.Models
{
    public class SacoRepository : ISacoRepository
    {
        sacoDataContext db = new sacoDataContext();

        public IQueryable<Employee> EmployeeList(string surname)
        {
            var EmployeeList = from e in db.Employees
                               where e.Surname.Contains(surname)
                               select e;

            return EmployeeList;
        }

        public IQueryable<Employee> EmployeeList(string surname, string firstname)
        {
            var EmployeeList = from e in db.Employees
                               where e.Surname.Contains(surname) && e.FirstName.Contains(firstname)
                               select e;

            return EmployeeList;
        }


        private String ToTime(int s)
        {
            int Minutes = s - (s % 60);


            int Hours = 0;
            int minutes = Minutes / 60;
            int seconds = s % 60;
            if (minutes > 59)
            {
                int temp = minutes;
                minutes = minutes % 60;
                Hours = (temp - minutes) / 60;

            }
            return FormatNumber(Hours) + ":" + FormatNumber(minutes) + ":" + FormatNumber(seconds);
        }

        private String FormatNumber(int num)
        {
            if (num < 10)
                return "0" + num.ToString();
            return num.ToString();


        }



        public List<day> SacoDays(string employeeNumber, DateTime SacoStartDate, DateTime SacoEndDate)
        {

            bool clockin = false;//the user has not clocked in yet
            bool clockout = false;// the user has not clocked out yet
            bool rowwritten = false;
            TimeSpan spanHoursWorked;
            TimeSpan dayTotal;

            DateTime TimeIn = DateTime.Now;
            DateTime TimeOut = DateTime.Now;

            string[] zones = { "DOG", "HTTTS", "TF", "ONSITE" };
            DateTime firstday = new DateTime(SacoStartDate.Year, SacoStartDate.Month, SacoStartDate.Day, 0,0,0);
            DateTime lastday = new DateTime(SacoEndDate.Year, SacoEndDate.Month, SacoEndDate.Day, 23, 59, 59);
                
         //       firstday.AddMonths(1).Subtract(TimeSpan.FromDays(1));
         //   lastday = new DateTime(SacoEndDate.Year, SacoEndDate.Month, SacoEndDate.Day, 23, 59, 59);
            


            var SacoDays = new List<day>();
            int i = 1;
            //Initialise all days to zero hours
            for (DateTime d = firstday; d < lastday; d = d.AddDays(1))
            {

                SacoDays.Add(new day() { Number=i,  Month = d.Month, Year = d.Year, Day = d.Day, HoursWorked = 0M });
                i++;
            }

            //Get all clocks to employee to month
            var q = from t in db.ClockHistories
                    join e in db.Employees on t.BadgeHolderNumber equals e.BadgeHolderNumber
                    join d in db.Readers on t.DeviceId equals d.DeviceId
                    where t.TimeStamp > firstday && t.TimeStamp <= lastday && t.AccessGranted == 'Y' && e.EmployeeNumber == employeeNumber
                    orderby t.TimeStamp
                    select new
                    {
                        TimeStamp = t.TimeStamp,
                        ResponseCode = t.ResponseCode,
                        Zone = d.ToZone.Trim().ToUpper()
                    };

            //loop through all days of month and find clock per day
            //then get total hours worked to that day
            foreach (var d in SacoDays)
            {
                dayTotal  = TimeSpan.Zero;
                if (q.Count() > 0)
                {
                    //Loop through the times

                     spanHoursWorked = TimeSpan.Zero;
                    foreach (var t in q.Where(t => t.TimeStamp.Year == d.Year && t.TimeStamp.Month == d.Month && t.TimeStamp.Day == d.Day))
                    {
                        if (zones.Contains(t.Zone.ToUpper()) && clockin == false)
                        {
                            TimeIn = t.TimeStamp;
                            clockin = true; //User clocks into first clock
                            rowwritten = false;
                        }

                        if (t.Zone.ToUpper() =="OFSITE" && rowwritten == false && clockin == true)
                        {
                            TimeOut = t.TimeStamp;
                            clockout = true;
                        }

                        if (clockin == true && clockout == true)
                        {
                            rowwritten = true;
                            clockin = false;
                            clockout = false;
                            spanHoursWorked = (TimeOut - TimeIn);
                            dayTotal += spanHoursWorked;
                           
                        }

                    }

                }

                if (dayTotal.TotalSeconds > 0)
                {
                    d.HoursWorked = (decimal)(dayTotal.TotalSeconds / 3600);
                    dayTotal = TimeSpan.Zero;
                }
            }

            return SacoDays;
        }

    }
}
