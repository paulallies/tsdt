using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TSDTReports.Models
{
    public interface IRoleRepository
    {
        List<tbl_User_Role> GetUserRoles();
        string GetUserRolesByUser(string User_CAI);
        
    }
}
