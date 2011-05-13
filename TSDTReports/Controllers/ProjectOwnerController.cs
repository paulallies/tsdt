using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using TSDTReports.Models;

namespace TSDTReports.Controllers
{
    public class ProjectOwnerController : Controller
    {
        IUserRepository userdb = new UserRepository();
        IProjectRepository projectdb = new ProjectRepository();
        IRoleRepository roledb = new RoleRepository();
        public ActionResult Index()
        {
            ViewData["Users"] = new SelectList(userdb.GetProjectOwnwersAndAdmins(), "cai", "detail");
            var role = roledb.GetUserRolesByUser(User.Identity.Name.ToString().ToUpper().Replace("CT\\",""))[0];
            if (!role.Equals("Administrator"))
                return View("AccessDenied");
            else
                return View();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Index(FormCollection collection, string submitbutton, string Search, int? page, int? size)
        {

            ViewData["Users"] = new SelectList(userdb.GetUserItems(), "cai", "detail", collection["User_CAI"]);
            //ViewData["ProjectsOwned"] = projectdb.GetOwnedProjects(collection["User_CAI"]);
            //ViewData["User_CAI"] = collection["User_CAI"];
            return View();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult InsertProject(int Project_ID, string User_CAI)
        {
            if (projectdb.AddProjectOwner(Project_ID, User_CAI))
                return Json(new { Status = true, Message = "Success" });
            else
                return Json(new { Status = false , Message = "Fail" });
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult DeleteProject(int Project_ID, string User_CAI)
        {
            if (projectdb.DeleteProjectOwner(Project_ID, User_CAI))
                return Json(new { Status = true, Message = "Success" });
            else
                return Json(new { Status = false, Message = "Fail" });
        }

        public ActionResult GridData(string sidx, string sord, int page, int rows, string User_CAI)
        {
            int pageIndex = Convert.ToInt32(page) - 1;
            int pageSize = rows;
            int totalRecords = projectdb.GetOwnedProjectsCount(User_CAI);
            int totalPages = (int)Math.Ceiling((float)totalRecords / (float)pageSize);

            int currentpage = page;
            int defaultPageSize = rows;

            var jsonData = new
            {
                total = totalPages,
                page = page,
                records = totalRecords,
                rows = (
                    from p in projectdb.GetProjectOwnedPaged(User_CAI, pageIndex, defaultPageSize, sidx, sord)
                    select new
                    {
                        id = p.Project_ID,
                        cell = new string[]
                        {
                            "",
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

        public ActionResult GetProjectOwners(int projectID)
        {
            try
            {
                var jsondata = new
                {
                    ProjectOwners =
                    (
                        from u in userdb.GetProjectOwnersByProjectID(projectID)
                        select new
                        {
                            u.User_CAI,
                            u.User_LastName,
                            u.User_FirstName
                        }
                    )
                };
                return Json(jsondata);
            }
            catch (Exception ex)
            {
                return Json(new { Status = false, Message = "Fail" });
            }
        }

        //[AcceptVerbs(HttpVerbs.Get)]
        //public ActionResult Delete(int Project_ID, string User_CAI)
        //{
        //    try
        //    {
        //        tsdtDataContext db = new tsdtDataContext();
        //        tbl_Project_Owner myPO = db.tbl_Project_Owners.Single(p => p.Project_ID == Project_ID && p.User_CAI == User_CAI);
        //        db.tbl_Project_Owners.DeleteOnSubmit(myPO);
        //        db.SubmitChanges();
        //    }
        //    catch { }



        //    ViewData["Users"] = new SelectList(userdb.GetUserItems(), "cai", "detail", User_CAI);
        //    ViewData["Projects"] = projectdb.GetOwnedProjects(User_CAI);
        //    ViewData["User_CAI"] = User_CAI;
        //    return View("Index");
        //}

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult ProjectList(string User_CAI)
        {

            ViewData["Users"] = new SelectList(userdb.GetUserItems(), "cai", "detail", User_CAI);
            // ViewData["Projects"] = projectdb.GetOwnedProjects(User_CAI);
            // ViewData["User_CAI"] = User_CAI;
            return View("Index");
        }

    }
}
