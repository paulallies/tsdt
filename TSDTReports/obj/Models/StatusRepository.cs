using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TSDTReports.Models
{
    public class StatusRepository : IStatusRepository
    {

        tsdtDataContext db = new tsdtDataContext();
        public List<tbl_Status> GetStatus()
        {
            return db.tbl_Status.ToList();
        }

    }
}
