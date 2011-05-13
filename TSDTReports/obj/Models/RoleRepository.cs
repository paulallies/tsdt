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

        public string GetUserRolesByUser(string User_CAI)
        {
            return db.tbl_Users.SingleOrDefault(u => u.User_CAI == User_CAI).tbl_User_Role.User_Role;
            
        }
    }
}
