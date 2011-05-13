using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TSDTReports.Models
{

    public class UserRepository : IUserRepository 
    {
        private tsdtDataContext db = new tsdtDataContext();

        public List<UserData> GetUsersHoursForProject(string[] project_ids, DateTime startDate, DateTime endDate)
        {
            List<UserData> myMonthUsers = new List<UserData>();



            var userlist = from t in db.tbl_TimeSheets
                           where project_ids.Contains(t.tbl_Resource.tbl_Project.Project_ID.ToString())
                           where (t.Date.Value >= startDate && t.Date.Value <= endDate)
                           group t by new
                           {
                               CAI = t.tbl_Resource.tbl_User.User_CAI,
                               FirstName = t.tbl_Resource.tbl_User.User_FirstName,
                               LastName = t.tbl_Resource.tbl_User.User_LastName
                           } into grp
                           select new
                           {
                               FirstName = grp.Key.FirstName,
                               LastName = grp.Key.LastName,
                               CAI = grp.Key.CAI,
                               hours = grp.Sum(o => o.Hours)

                           };


            foreach (var u in userlist)
            {
                UserData newdata = new UserData();
                newdata.CAI = u.CAI;
                newdata.hours = u.hours;
                newdata.FirstName = u.FirstName;
                newdata.LastName = u.LastName;

                myMonthUsers.Add(newdata);
            }

            //var currentProject = db.tbl_Project.Single(p => p.Project_ID == project_id );

            //var package = new ProjectDetails()
            //{
            //    Project_Number = currentProject.Project_Number,
            //    StartDate = startDate.ToString("dd-MMM-yyyy"),
            //    EndDate = endDate.ToString("dd-MMM-yyyy"),
            //    Users = myMonthUsers,
            //    Project_Name = currentProject.Project_Name,
            //    Project_Status = currentProject.tbl_Status.Status,
            //    TotalUserCount = myMonthUsers.Count(),
            //    TotalHourCount = myMonthUsers.Sum(u => u.hours),

            //};

            return myMonthUsers;
        }

        public IQueryable<UserItem> GetUserItems()
        {
            var users = from u in db.tbl_Users
                        where(u.active == true)
                        orderby u.User_LastName
                        select new UserItem
                        {
                            cai = u.User_CAI,
                            detail = u.User_LastName + ", " + u.User_FirstName + " [" + u.User_CAI + "]"
                        };
            return users;
        }

        public tbl_User GetUserByID(string cai)
        {
            return db.tbl_Users.Single(u => u.User_CAI == cai);
        }

        public void Save()
        {
            db.SubmitChanges();
        }

        public void AddUser(tbl_User myUser)
        {
            db.tbl_Users.InsertOnSubmit(myUser);
        }

        public void DeleteUser(tbl_User myUser)
        {
            db.tbl_Users.DeleteOnSubmit(myUser);
        }

        public List<tbl_User> GetUserPaged(int currentPageIndex, int defaultPageSize, string User_CAI)
        {
            return db.tbl_Users.Where(u => u.User_CAI  == User_CAI).Skip(currentPageIndex * defaultPageSize).Take(defaultPageSize).ToList();

        }

        public int GetUserPageCount(int Pagesize, string User_CAI)
       {
             return db.tbl_Users.Where(u => u.User_CAI == User_CAI).Count() / Pagesize;

        }

        public IQueryable<tbl_User> GetUsers()
        {
            return db.tbl_Users;
        }

        public IQueryable<tbl_User> GetUsersPaged(int currentPageIndex, int defaultPageSize, string sidx, string sord)
        {
            var Users = this.GetUsers();
            return Sort(ref Users, currentPageIndex, defaultPageSize, sidx, sord);
            
        }
        
        public IQueryable<tbl_User> GetUsersByCAI(string User_CAI)
        {
            return db.tbl_Users.Where(u => u.User_CAI == User_CAI);
        }

        public IQueryable<tbl_User> GetUsersPagedByCAI(int pageIndex, int defaultPageSize, string sidx, string sord, string User_CAI)
        {
            var Users = this.GetUsers().Where(u => u.User_CAI == User_CAI);
            return Sort(ref Users, pageIndex, defaultPageSize, sidx, sord);    
        }

        private IQueryable<tbl_User> Sort(ref IQueryable<tbl_User> Users, int pageIndex, int defaultPageSize, string sidx, string sord)
        {
            switch (sidx.ToUpper())
            {
                case "USER_CAI":
                    if (sord == "asc") Users = Users.OrderBy(i => i.User_CAI);
                    if (sord == "desc") Users = Users.OrderByDescending(i => i.User_CAI);
                    break;

                case "USER_FIRSTNAME":
                    if (sord == "asc") Users = Users.OrderBy(i => i.User_FirstName);
                    if (sord == "desc") Users = Users.OrderByDescending(i => i.User_FirstName);
                    break;

                case "USER_LASTNAME":
                    if (sord == "asc") Users = Users.OrderBy(i => i.User_LastName);
                    if (sord == "desc") Users = Users.OrderByDescending(i => i.User_LastName);
                    break;

                case "USER_ROLE":
                    if (sord == "asc") Users = Users.OrderBy(i => i.tbl_User_Role.User_Role_Descr);
                    if (sord == "desc") Users = Users.OrderByDescending(i => i.tbl_User_Role.User_Role_Descr);
                    break;

                case "ACTIVE":
                    if (sord == "asc") Users = Users.OrderBy(i => i.active);
                    if (sord == "desc") Users = Users.OrderByDescending(i => i.active);
                    break;

            }

            return Users.Skip(pageIndex * defaultPageSize).Take(defaultPageSize);
        }

        public IQueryable<tbl_User> GetActiveUsers()
        {
            return db.tbl_Users.Where(u => u.active == true);
        }

        public List<UserItem> GetProjectOwnwersAndAdmins()
        {
            var users = from u in db.tbl_Users
                        where (u.tbl_User_Role.User_Role == "Administrator" || u.tbl_User_Role.User_Role == "ProjectOwner")
                        orderby u.User_LastName
                        select new
                        {
                            cai = u.User_CAI,
                            detail = u.User_LastName + ", " + u.User_FirstName + " [" + u.User_CAI + "] [" + u.tbl_User_Role.User_Role_Descr + "]"
                        };

            List<UserItem> myList = new List<UserItem>();

            foreach (var item in users)
            {

                myList.Add(new UserItem()
                {
                    cai = item.cai,
                    detail = item.detail
                });
            }

            return myList;
        }

        public IQueryable<tbl_User> GetSearchedUsers(string searchField, string searchString, string searchOper)
        {
            var predicate = PredicateExtensions.True<tbl_User>();

            switch (searchField)
            {
                case "User_CAI":
                    if (searchOper == "bw") predicate = predicate.And(p => p.User_CAI.ToString().StartsWith(searchString));
                    if (searchOper == "eq") predicate = predicate.And(p => p.User_CAI == searchString);
                    if (searchOper == "cn") predicate = predicate.And(p => p.User_CAI.ToString().Contains(searchString));
                    break;
                case "User_FirstName":
                    if (searchOper == "bw") predicate = predicate.And(p => p.User_FirstName.ToString().StartsWith(searchString));
                    if (searchOper == "eq") predicate = predicate.And(p => p.User_FirstName == searchString);
                    if (searchOper == "cn") predicate = predicate.And(p => p.User_FirstName.ToString().Contains(searchString));
                    break;
                case "User_LastName":
                    if (searchOper == "bw") predicate = predicate.And(p => p.User_LastName.StartsWith(searchString));
                    if (searchOper == "eq") predicate = predicate.And(p => p.User_LastName == searchString);
                    if (searchOper == "cn") predicate = predicate.And(p => p.User_LastName.ToString().Contains(searchString));
                    break;
                case "User_Role":
                    if (searchOper == "bw") predicate = predicate.And(p => p.tbl_User_Role.User_Role_Descr.StartsWith(searchString));
                    if (searchOper == "eq") predicate = predicate.And(p => p.tbl_User_Role.User_Role_Descr == searchString);
                    if (searchOper == "cn") predicate = predicate.And(p => p.tbl_User_Role.User_Role_Descr.ToString().Contains(searchString));
                    break;
                
            }
            return db.tbl_Users.Where(predicate);
        }

        public IQueryable<tbl_User> GetSearchedUsersPaged(int pageIndex, int defaultPageSize, string sidx, string sord, string searchField, string searchString, string searchOper)
        {
            var Users = this.GetSearchedUsers(searchField, searchString, searchOper);
            return Sort(ref Users, pageIndex, defaultPageSize, sidx, sord);
        }

        public IQueryable<tbl_Resource> GetUserResources(string User_CAI)
        {
            var UserRes = from r in db.tbl_Resources
                          where (r.User_CAI == User_CAI)
                          orderby (r.tbl_Project.Project_Number)
                          select r;
            return UserRes;
        }

        public IQueryable<tbl_User> GetProjectOwnersByProjectID(int ProjectID)
        {
            var ProjectOwners = from p in db.tbl_Project_Owners
                                where p.Project_ID == ProjectID
                                orderby p.tbl_User.User_LastName
                                select p.tbl_User;

            return ProjectOwners;
        }

    }
}
