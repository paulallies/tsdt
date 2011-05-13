using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections;

namespace TSDTReports.Models
{
    public interface IProjectRepository
    {
        tbl_Project GetProjectByID(int project_id);
        int GetProjectsCount();
        List<ProjectItem> GetProjectItems();
        List<ProjectHours> GetProjectHours(string CAI, DateTime startDate, DateTime endDate);
        List<tbl_Project> GetProjectsByProjectNumber(int ProjectNumber);
        List<tbl_Project> GetProjectsByIds(string[] project_ids);
        List<tbl_Project> GetProjectPaged(int currentPageIndex, int defaultPageSize);
        int GetProjectPageCount(int Pagesize);
        List<tbl_Project> GetProjectPaged(int currentPageIndex, int defaultPageSize, int ProjectNumber);
        int GetProjectPageCount(int Pagesize, int ProjectNumber);
        int GetOwnedProjectsCount(string User_CAI);
        List<tbl_Project> GetProjectOwnedPaged(string User_CAI, int currentPageIndex, int defaultPageSize, string sidx, string sord);
        bool AddProjectOwner(int Project_ID, string User_CAI);
        void Add(tbl_Project newProject);
        void Delete(tbl_Project myProject);
        void Save();
        bool DeleteProjectOwner(int Project_ID, string User_CAI);

        List<tbl_Project> GetOwnerProjects(string User_CAI);

        IQueryable<tbl_Project> GetAllProjects();
        IQueryable<tbl_Project> GetAllProjectsByProjectNumber(int Project_Number);
        IQueryable<tbl_Project> GetAllProjectsPaged(int pageIndex, int defaultPageSize, string sidx, string sord);
        IQueryable<tbl_Project> GetAllProjectsPagedByProjectNumber(int Project_Number, int pageIndex, int defaultPageSize, string sidx, string sord);


        IQueryable<tbl_Project> GetSearchedProjects(string searchField, string searchString, string searchOper);

        IQueryable<tbl_Project> GetAllProjectsPaged(int pageIndex, int defaultPageSize, string sidx, string sord, string searchField, string searchString, string searchOper);
    }

    public class ProjectHours
    {
        public int? ProjectID { get; set; }

        public int? ProjectNumber { get; set; }

        public string ProjectName { get; set; }

        public string ProjectStatus { get; set; }

        public decimal? Hours { get; set; }
    }



    public class ProjectItem
    {
        public int id { get; set; }
        public string detail { get; set; }
    }





}
