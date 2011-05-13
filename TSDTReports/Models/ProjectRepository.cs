using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections;

namespace TSDTReports.Models
{
    public class ProjectRepository : IProjectRepository
    {
        private tsdtDataContext db = new tsdtDataContext();

        public tbl_Project GetProjectByID(int project_id)
        {
            return db.tbl_Projects.First(p => p.Project_ID == project_id);
        }

        public List<ProjectItem> GetProjectItems()
        {
            var projects = from p in db.tbl_Projects
                           orderby p.Project_Number
                           select new
                           {
                               id = p.Project_ID,
                               detail = p.Project_Number + " - " + p.Project_Name
                           };

            List<ProjectItem> myList = new List<ProjectItem>();



            foreach (var item in projects)
            {

                myList.Add(new ProjectItem()
                {
                    id = item.id,
                    detail = item.detail
                });
            }

            return myList;
        }

        public List<ProjectHours> GetProjectHours(string CAI, DateTime startDate, DateTime endDate)
        {
            //var UserProjects = from t in db.tbl_Resources
            //                   where t.User_CAI == CAI
            //                   select new
            //                   {
            //                       ProjectID = t.tbl_Project.Project_ID,
            //                       ProjectName = t.tbl_Project.Project_Name,
            //                       ProjectNumber = t.tbl_Project.Project_Number,
            //                       ProjectStatus = t.tbl_Project.tbl_Status.Status
            //                   };

            var ProjectHours = from t in db.tbl_TimeSheets
                               where (t.tbl_Resource.User_CAI == CAI && t.Date >= startDate && t.Date <= endDate && t.tbl_Resource.tbl_Project.Project_Number != null)
                               group t by new
                               {
                                   ProjectID = t.tbl_Resource.Project_ID,
                                   ProjectNumber = t.tbl_Resource.tbl_Project.Project_Number,
                                   ProjectName = t.tbl_Resource.tbl_Project.Project_Name,
                                   ProjectStatus = t.tbl_Resource.tbl_Project.tbl_Status.Status
                               } into grp
                               select new
                               {
                                   PID = grp.Key.ProjectID,
                                   PNumber = grp.Key.ProjectNumber,
                                   PName = grp.Key.ProjectName,
                                   PStatus = grp.Key.ProjectStatus,
                                   Hours = grp.Sum(h => h.Hours)
                               };

            var package = new List<ProjectHours>();

            foreach (var ph in ProjectHours)
            {
                package.Add(new ProjectHours()
                {
                    ProjectID = ph.PID,
                    ProjectNumber = ph.PNumber,
                    ProjectName = ph.PName,
                    ProjectStatus = ph.PStatus,
                    Hours = ph.Hours
                });
            }

            return package;
        }

        public List<tbl_Project> GetProjectsByProjectNumber(int ProjectNumber)
        {
            return db.tbl_Projects.Where(p => p.Project_Number == ProjectNumber).ToList();
        }

        public List<tbl_Project> GetProjectsByIds(string[] project_ids)
        {

            var projectlist = from p in db.tbl_Projects
                              where project_ids.Contains(p.Project_ID.ToString())
                              select p;

            return projectlist.ToList();
        }

        public List<tbl_Project> GetProjectPaged(int currentPageIndex, int defaultPageSize)
        {
            return db.tbl_Projects.Skip(currentPageIndex * defaultPageSize).Take(defaultPageSize).ToList();
        }

        public int GetProjectPageCount(int Pagesize)
        {
            return db.tbl_Projects.Count() / Pagesize;
        }

        public List<tbl_Project> GetProjectPaged(int currentPageIndex, int defaultPageSize, int ProjectNumber)
        {
            return db.tbl_Projects.Where(p => p.Project_Number == ProjectNumber).Skip(currentPageIndex * defaultPageSize).Take(defaultPageSize).ToList();
        }

        public int GetProjectPageCount(int Pagesize, int ProjectNumber)
        {
            return db.tbl_Projects.Where(p => p.Project_Number == ProjectNumber).Count() / Pagesize;
        }

        public int GetOwnedProjectsCount(string User_CAI)
        {
            var OP = from p in db.tbl_Project_Owners
                     where p.User_CAI == User_CAI
                     select p;

            return OP.Count();

        }

        public void Add(tbl_Project newProject)
        {
            db.tbl_Projects.InsertOnSubmit(newProject);
        }

        public void Save()
        {
            db.SubmitChanges();
        }

        public void Delete(tbl_Project myProject)
        {
            db.tbl_Projects.DeleteOnSubmit(myProject);

        }

        public int GetProjectsCount()
        {
            return db.tbl_Projects.Count();
        }

        public List<tbl_Project> GetProjectOwnedPaged(string User_CAI, int currentPageIndex, int defaultPageSize, string sidx, string sord)
        {
            var OP = from p in db.tbl_Project_Owners
                     where p.User_CAI == User_CAI
                     select p;


            switch (sidx)
            {
                case "ID":
                    if (sord == "asc") OP = OP.OrderBy(p => p.Project_ID);
                    if (sord == "desc") OP = OP.OrderByDescending(p => p.Project_ID);
                    break;

                case "Project_Number":
                    if (sord == "asc") OP = OP.OrderBy(p => p.tbl_Project.Project_Number);
                    if (sord == "desc") OP = OP.OrderByDescending(p => p.tbl_Project.Project_Number);
                    break;

                case "SAP_Number":
                    if (sord == "asc") OP = OP.OrderBy(p => p.tbl_Project.SAP_Number);
                    if (sord == "desc") OP = OP.OrderByDescending(p => p.tbl_Project.SAP_Number);
                    break;

                case "WBS":
                    if (sord == "asc") OP = OP.OrderBy(p => p.tbl_Project.WBS);
                    if (sord == "desc") OP = OP.OrderByDescending(p => p.tbl_Project.WBS);
                    break;

                case "Project_Name":
                    if (sord == "asc") OP = OP.OrderBy(p => p.tbl_Project.Project_Name);
                    if (sord == "desc") OP = OP.OrderByDescending(p => p.tbl_Project.Project_Name);
                    break;

                case "Status":
                    if (sord == "asc") OP = OP.OrderBy(p => p.tbl_Project.tbl_Status.Status);
                    if (sord == "desc") OP = OP.OrderByDescending(p => p.tbl_Project.tbl_Status.Status);
                    break;

            }

            OP = OP.Skip(currentPageIndex * defaultPageSize).Take(defaultPageSize);

            List<tbl_Project> myProjects = new List<tbl_Project>();

            foreach (var op in OP)
            {
                myProjects.Add(GetProjectByID(op.Project_ID));
            }

            return myProjects;
        }

        public bool AddProjectOwner(int Project_ID, string User_CAI)
        {
            try
            {
                tbl_Project_Owner myProjectOwner = new tbl_Project_Owner();
                myProjectOwner.Project_ID = Project_ID;
                myProjectOwner.User_CAI = User_CAI;
                db.tbl_Project_Owners.InsertOnSubmit(myProjectOwner);
                db.SubmitChanges();
                return (myProjectOwner.id > 0);
            }
            catch
            {
                return (false);
            }
        }

        public bool DeleteProjectOwner(int Project_ID, string User_CAI)
        {
            try
            {
                tbl_Project_Owner myProjectOwner = db.tbl_Project_Owners.Single(p => p.Project_ID == Project_ID && p.User_CAI == User_CAI);
                db.tbl_Project_Owners.DeleteOnSubmit(myProjectOwner);
                db.SubmitChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public List<tbl_Project> GetOwnerProjects(string User_CAI)
        {
            var OP = (from p in db.tbl_Project_Owners
                      where p.User_CAI == User_CAI
                      select p).ToList();

            List<tbl_Project> myProjects = new List<tbl_Project>();

            foreach (var op in OP)
            {
                myProjects.Add(GetProjectByID(op.Project_ID));
            }

            return myProjects;
        }

        public IQueryable<tbl_Project> GetAllProjects()
        {
            return db.tbl_Projects;
        }

        public IQueryable<tbl_Project> GetAllProjectsByProjectNumber(int Project_Number)
        {
            return db.tbl_Projects.Where(p => p.Project_Number == Project_Number);
        }

        public IQueryable<tbl_Project> GetAllProjectsPaged(int pageIndex, int defaultPageSize, string sidx, string sord)
        {
            var Projects = this.GetAllProjects();
            return Sort(ref Projects, pageIndex, defaultPageSize, sidx, sord);
        }

        public IQueryable<tbl_Project> GetAllProjectsPagedByProjectNumber(int ProjectNumber, int pageIndex, int defaultPageSize, string sidx, string sord)
        {
            var Projects = this.GetAllProjectsByProjectNumber(ProjectNumber);
            return Sort(ref Projects, pageIndex, defaultPageSize, sidx, sord);

        }

        private IQueryable<tbl_Project> Sort(ref IQueryable<tbl_Project> Projects, int pageIndex, int defaultPageSize, string sidx, string sord)
        {
            switch (sidx)
            {
                case "ID":
                    if (sord == "asc") Projects = Projects.OrderBy(i => i.Project_ID);
                    if (sord == "desc") Projects = Projects.OrderByDescending(i => i.Project_ID);
                    break;

                case "Project_Number":
                    if (sord == "asc") Projects = Projects.OrderBy(i => i.Project_Number);
                    if (sord == "desc") Projects = Projects.OrderByDescending(i => i.Project_Number);
                    break;

                case "SAP_Number":
                    if (sord == "asc") Projects = Projects.OrderBy(i => i.SAP_Number);
                    if (sord == "desc") Projects = Projects.OrderByDescending(i => i.SAP_Number);
                    break;

                case "WBS":
                    if (sord == "asc") Projects = Projects.OrderBy(i => i.WBS);
                    if (sord == "desc") Projects = Projects.OrderByDescending(i => i.WBS);
                    break;


                case "Project_Name":
                    if (sord == "asc") Projects = Projects.OrderBy(i => i.Project_Name);
                    if (sord == "desc") Projects = Projects.OrderByDescending(i => i.Project_Name);
                    break;


                case "Status":
                    if (sord == "asc") Projects = Projects.OrderBy(i => i.tbl_Status.Status);
                    if (sord == "desc") Projects = Projects.OrderByDescending(i => i.tbl_Status.Status);
                    break;
            }

            return Projects.Skip(pageIndex * defaultPageSize).Take(defaultPageSize);
        }


        public IQueryable<tbl_Project> GetSearchedProjects(string searchField, string searchString, string searchOper)
        {
            var predicate = PredicateExtensions.True<tbl_Project>();

            switch (searchField)
            {
                case "ID":
                    if (searchOper == "bw") predicate = predicate.And(p => p.Project_ID.ToString().StartsWith(searchString));
                    if (searchOper == "eq") predicate = predicate.And(p => p.Project_ID == int.Parse(searchString));
                    if (searchOper == "cn") predicate = predicate.And(p => p.Project_ID.ToString().Contains(searchString));
                    break;
                case "Project_Number":
                    if (searchOper == "bw") predicate = predicate.And(p => p.Project_Number.ToString().StartsWith(searchString));
                    if (searchOper == "eq") predicate = predicate.And(p => p.Project_Number == int.Parse(searchString));
                    if (searchOper == "cn") predicate = predicate.And(p => p.Project_Number.ToString().Contains(searchString));
                    break;
                case "SAP_Number":
                    if (searchOper == "bw") predicate = predicate.And(p => p.SAP_Number.StartsWith(searchString));
                    if (searchOper == "eq") predicate = predicate.And(p => p.SAP_Number == searchString);
                    if (searchOper == "cn") predicate = predicate.And(p => p.SAP_Number.ToString().Contains(searchString));
                    break;
                case "WBS":
                    if (searchOper == "bw") predicate = predicate.And(p => p.WBS.StartsWith(searchString));
                    if (searchOper == "eq") predicate = predicate.And(p => p.WBS == searchString);
                    if (searchOper == "cn") predicate = predicate.And(p => p.WBS.ToString().Contains(searchString));

                    break;
                case "Project_Name":
                    if (searchOper == "bw") predicate = predicate.And(p => p.Project_Name.StartsWith(searchString));
                    if (searchOper == "eq") predicate = predicate.And(p => p.Project_Name == searchString);
                    if (searchOper == "cn") predicate = predicate.And(p => p.Project_Name.ToString().Contains(searchString));
                    break;
                case "Status":
                    if (searchOper == "bw") predicate = predicate.And(p => p.tbl_Status.Status.StartsWith(searchString));
                    if (searchOper == "eq") predicate = predicate.And(p => p.tbl_Status.Status == searchString);
                    if (searchOper == "cn") predicate = predicate.And(p => p.tbl_Status.Status.Contains(searchString));
                    break;
            }
            return db.tbl_Projects.Where(predicate);
        }

        public IQueryable<tbl_Project> GetAllProjectsPaged(int pageIndex, int defaultPageSize, string sidx, string sord, string searchField, string searchString, string searchOper)
        {
            var Projects = this.GetSearchedProjects(searchField, searchString, searchOper);
            return Sort(ref Projects, pageIndex, defaultPageSize, sidx, sord);
        }

    }
}
