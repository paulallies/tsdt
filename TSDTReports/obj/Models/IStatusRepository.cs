using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TSDTReports.Models
{
    public interface IStatusRepository
    {
        List<tbl_Status> GetStatus();
    }
}
