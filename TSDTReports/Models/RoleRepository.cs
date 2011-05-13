using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TSDTReports.Models
{
    public class RoleRepository : IRoleRepository
    {
        tsdtDataContext db = new tsdtDataContext();

        public List<tbl_User_Role> GetUserRoles()
        {
            return db.tbl_User_Roles.ToList();
        }

        public string[] GetUserRolesByUser(string User_CAI)
        {
            var q = from u in db.tbl_Users
                    where u.User_CAI == User_CAI
                    select u.tbl_User_Role.User_Role;

            return q.ToArray();
            
        }
    }
}
