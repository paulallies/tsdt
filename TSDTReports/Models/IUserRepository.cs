using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TSDTReports.Models
{



    public interface IUserRepository
    {


        List<UserData> GetUsersHoursForProject(string[] project_ids, DateTime startDate, DateTime endDate);
        tbl_User GetUserByID(string cai);
        IQueryable<UserItem> GetUserItems();
        
        void AddUser(tbl_User myUser);
        void DeleteUser(tbl_User myUser);
        void Save();
        List<UserItem> GetProjectOwnwersAndAdmins();
        IQueryable<tbl_User> GetActiveUsers();
        IQueryable<tbl_User> GetUsers();
        IQueryable<tbl_User> GetUsersPaged(int currentPageIndex, int defaultPageSize , string sidx, string sord);
        IQueryable<tbl_User> GetUsersByCAI(string User_CAI);
        IQueryable<tbl_User> GetUsersPagedByCAI(int pageIndex, int defaultPageSize, string sidx, string sord, string User_CAI);

        IQueryable<tbl_User> GetSearchedUsers(string searchField, string searchString, string searchOper);

        IQueryable<tbl_User> GetSearchedUsersPaged(int pageIndex, int defaultPageSize, string sidx, string sord, string searchField, string searchString, string searchOper);
        IQueryable<tbl_Resource> GetUserResources(string User_CAI);
        IQueryable<tbl_User> GetProjectOwnersByProjectID(int ProjectID);

    }

    public class UserData
    {
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string CAI { get; set; }
        public decimal? hours { get; set; }
    }

    public class UserItem
    {
        public string cai { get; set; }
        public string detail { get; set; }
    }
}
