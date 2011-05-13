using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using System.Web.Services;
using TSDTReports.Models;

namespace TSDTReports.Controllers
{

    public class ProjectController : Controller
    {
        IProjectRepository db = new ProjectRepository();
        IStatusRepository statusdb = new StatusRepository();
        IRoleRepository roledb = new RoleRepository();

        public ActionResult List()
        {
            return View();
        }

        public ActionResult GridData(string sidx, string sord, int page, int rows, bool _search, string searchField, string searchOper, string searchString)
        {
            int pageIndex = Convert.ToInt32(page) - 1;
            int pageSize = rows;
            int totalRecords = db.GetProjectsCount();
            if (_search == true)
            {
                totalRecords = db.GetSearchedProjects(searchField, searchString, searchOper).Count();
            }
            int totalPages = (int)Math.Ceiling((float)totalRecords / (float)pageSize);
            int currentpage = page;
            int defaultPageSize = rows;
            var pagedProjects = db.GetAllProjectsPaged(pageIndex, defaultPageSize, sidx, sord);
            if (_search == true)
            {
                pagedProjects = db.GetAllProjectsPaged(pageIndex, defaultPageSize, sidx, sord, searchField, searchString, searchOper);
            }

            var jsonData = new
            {
                total = totalPages,
                page = page,
                records = totalRecords,
                rows = (
                    from p in pagedProjects
                    select new
                    {
                        id = p.Project_ID,
                        cell = new string[]
                        {
                            p.Project_ID.ToString(),
                            p.Project_Number.ToString(),
                            p.SAP_Number,
                            p.WBS,
                            p.Project_Name,
                            p.tbl_Status.Status
                        }
                    }).ToArray()
            };
            return Json(jsonData);
        }


        public ActionResult Index()
        {
            ViewData["Title"] = "Project List";
            string role = roledb.GetUserRolesByUser(User.Identity.Name.ToString().ToUpper().Replace("CT\\",""))[0];
            if (!role.Equals("Administrator"))
                return View("AccessDenied");
           else
                return View();
        }

        

        public ActionResult GetAllProjects(string sidx, string sord, int page, int rows, bool _search, string searchField, string searchOper, string searchString)
        {
            int pageIndex = Convert.ToInt32(page) - 1;
            int pageSize = rows;
            int totalRecords = db.GetAllProjects().Count();
            if (_search)
            {
                totalRecords = db.GetSearchedProjects(searchField, searchString, searchOper).Count();
            }
            int totalPages = (int)Math.Ceiling((float)totalRecords / (float)pageSize);
            int currentpage = page;
            int defaultPageSize = rows;
            var pagedProjects = db.GetAllProjectsPaged(pageIndex, defaultPageSize, sidx, sord);
            if (_search)
            {
                pagedProjects = db.GetAllProjectsPaged(pageIndex, defaultPageSize, sidx, sord, searchField, searchString, searchOper);
            }

            var jsonData = new
            {
                total = totalPages,
                page = page,
                records = totalRecords,
                rows = (
                    from p in pagedProjects
                    select new
                    {
                        id = p.Project_ID,
                        cell = new string[]
                        {
                            "",//include this for actions
                            p.Project_ID.ToString(),
                            p.Project_Number.ToString(),
                            p.SAP_Number,
                            p.WBS,
                            p.Project_Name,
                            p.tbl_Status.Status
                        }
                    }).ToArray()

            };
            return Json(jsonData);
        }



        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Create()
        {
            ViewData["Status"] = new SelectList(statusdb.GetStatus(), "Status_ID", "Status");

            return View();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Create(tbl_Project myProject)
        {
            ViewData["Status"] = new SelectList(statusdb.GetStatus(), "Status_ID", "Status");
            if (!myProject.Project_Number.HasValue)
                ModelState.AddModelError("Project_Number", "Project Number is Required");
            if (myProject.SAP_Number.Trim().Length == 0)
                ModelState.AddModelError("SAP_Number", "SAP Number is Required");
            if (myProject.WBS.Trim().Length == 0)
                ModelState.AddModelError("WBS", "WBS is Required");
            if (myProject.Project_Name.Trim().Length == 0)
                ModelState.AddModelError("Project_Name", "Project Name is Required");
            if (!myProject.Status_ID.HasValue)
                ModelState.AddModelError("Status_ID", "Status is Required");
            if (!ModelState.IsValid)
            {
                return View();
            }

            try
            {
                db.Add(myProject);
                db.Save();
                return RedirectToAction("Index");

            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Error: " + ex.Message);
                return View();
            }
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Details(int Project_ID, string Error)
        {
            try
            {
                ViewData["Error"] = Error;
                return View(db.GetProjectByID(Project_ID));
            }
            catch
            {
                return RedirectToAction("Index");
            }
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Delete(int Project_ID)
        {
            try
            {
                tbl_Project dbItem = db.GetProjectByID(Project_ID);
                db.Delete(dbItem);
                db.Save();
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Error: " + ex.Message);

                return RedirectToAction("Details", new { Project_ID = Project_ID, Error = "Error deleting Project!" });
            }
        }


        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Edit(int Project_ID)
        {
            tbl_Project myProject = db.GetProjectByID(Project_ID);
            ViewData["Status"] = new SelectList(statusdb.GetStatus(), "Status_ID", "Status", myProject.Status_ID);
            return View(myProject);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Edit(tbl_Project myProject)
        {
            if (!myProject.Project_Number.HasValue)
                ModelState.AddModelError("Project_Number", "Project Number is Required");

            if (myProject.SAP_Number.Length == 0)
                ModelState.AddModelError("SAP_Number", "SAP Number Required");

            if (myProject.WBS.Trim().Length == 0)
                ModelState.AddModelError("WBS", "WBS Required");

            if (myProject.Project_Name.Trim().Length == 0)
                ModelState.AddModelError("Project_Name", "Project Name Required");

            if (!ModelState.IsValid)
            {
                ViewData["Status"] = new SelectList(statusdb.GetStatus(), "Status_ID", "Status", myProject.Status_ID);

                return View(myProject);
            }

            try
            {
                tbl_Project dbProject = db.GetProjectByID(myProject.Project_ID);
                dbProject.Project_Number = myProject.Project_Number;
                dbProject.SAP_Number = myProject.SAP_Number;
                dbProject.Project_Name = myProject.Project_Name;
                dbProject.WBS = myProject.WBS;
                dbProject.Status_ID = myProject.Status_ID;

                db.Save();
                return RedirectToAction("Details", new { Project_ID = myProject.Project_ID });

            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Error: " + ex.Message);
                return View();
            }
        }
    }


}
