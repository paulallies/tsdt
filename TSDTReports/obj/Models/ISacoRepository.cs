using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TSDTReports.Models
{
    public interface ISacoRepository
    {
        IQueryable<Employee> EmployeeList(string surname);
        IQueryable<Employee> EmployeeList(string surname, string firstname);
        List<day> SacoDays(string employeeNumber, DateTime SacoStartDate, DateTime SacoEndDate);
    }
}
