using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using TSDTReports.Models;

namespace TSDTReports.Controllers
{
    public class MyProjectController : Controller
    {
        IUserRepository userdb = new UserRepository();
        IProjectRepository projectdb = new ProjectRepository();
        tsdtDataContext db = new tsdtDataContext();
        IResourceRepository resourcedb = new ResourceRepository();
        IRoleRepository roledb = new RoleRepository();


        public ActionResult Index()
        {
            ViewData["Users"] = new SelectList(userdb.GetProjectOwnwersAndAdmins(), "cai", "detail", HttpContext.User.Identity.Name.ToUpper().Replace("CT\\", ""));
            string role = roledb.GetUserRolesByUser(User.Identity.Name.ToString().ToUpper().Replace("CT\\",""));
            ViewData["Role"] = role;

            if (!role.Equals("Administrator") && (!role.Equals("ProjectOwner")))
                return View("AccessDenied");
            else
                return View();
        }

        public ActionResult GetOwnerProjects(string User_CAI)
        {
            var JsonData = new
            {
                data = 
                (
                    from p in projectdb.GetOwnerProjects(User_CAI)
                    select new 
                    {
                       id =  p.Project_ID,
                       details = ((int)p.Project_Number).ToString("0000#") + " | " +  p.WBS + " | " + p.Project_Name
                    }               
                ).ToArray()
            };

            return Json(JsonData);
        }



        public ActionResult GetProjectResources(string sidx, string sord, int page, int rows, int Project_ID, bool _search, string searchField, string searchOper, string searchString)
        {
            int pageIndex = Convert.ToInt32(page) - 1;
            int pageSize = rows;

            var projectresources = from pr in db.tbl_Resources
                                   where (pr.Project_ID == Project_ID)
                                   select new 
                                   {
                                       id = pr.Resource_ID,
                                       cai = pr.User_CAI,
                                       lastName = pr.tbl_User.User_LastName,
                                       FirstName = pr.tbl_User.User_FirstName,
                                       Active = pr.active
                                   };

            switch (sidx.ToUpper())
            {
                case "ID":
                    if (sord == "asc") projectresources = projectresources.OrderBy(i => i.id);
                    if (sord == "desc") projectresources = projectresources.OrderByDescending(i => i.id);
                    break;

                case "USER_CAI":
                    if (sord == "asc") projectresources = projectresources.OrderBy(i => i.cai);
                    if (sord == "desc") projectresources = projectresources.OrderByDescending(i => i.cai);
                    break;

                case "USER_LASTNAME":
                    if (sord == "asc") projectresources = projectresources.OrderBy(i => i.lastName);
                    if (sord == "desc") projectresources = projectresources.OrderByDescending(i => i.lastName);
                    break;

                case "USER_FIRSTNAME":
                    if (sord == "asc") projectresources = projectresources.OrderBy(i => i.FirstName);
                    if (sord == "desc") projectresources = projectresources.OrderByDescending(i => i.FirstName);
                    break;


                case "ACTIVE":
                    if (sord == "asc") projectresources = projectresources.OrderBy(i => i.Active);
                    if (sord == "desc") projectresources = projectresources.OrderByDescending(i => i.Active);
                    break;

            }

            int totalRecords = projectresources.Count();
            int totalPages = (int)Math.Ceiling((float)totalRecords / (float)pageSize);

            int currentpage = page;
            int defaultPageSize = rows;

            var jsonData = new
            {
                total = totalPages,
                page = page,
                records = totalRecords,
                rows = (
                    from p in projectresources.Skip(pageIndex*defaultPageSize).Take(defaultPageSize)
                    select new
                    {
                        id = p.cai,
                        cell = new string[]
                        {
                            p.id.ToString(),
                            p.cai,     
                            p.FirstName,
                            p.lastName,
                            p.Active.ToString()
                        }
                    }).ToArray()

            };
            return Json(jsonData);
        }

        public ActionResult InsertProjectResource(int Project_ID, string User_CAI)
        {
            try
            {
                if (resourcedb.FindResource(Project_ID, User_CAI).Count() > 0)
                {
                    return Json(new { Status = false, Message = "Fail" });
                }
                else
                {
                    tbl_Resource resource = new tbl_Resource();
                    resource.User_CAI = User_CAI;
                    resource.Project_ID = Project_ID;
                    resource.active = true;
                    resourcedb.Add(resource);
                    resourcedb.Save();
                    return Json(new { Status = true, Message = "Success" });
                }

            }
            catch(Exception ex)
            {
                return Json(new { Status = false, Message = "Fail: " + ex.Message });
            }
              
        }

        public ActionResult DeleteProjectResource(int resource_ID)
        {
            try
            {
                tbl_Resource resource = resourcedb.GetResource(resource_ID);
                resourcedb.Delete(resource);
                resourcedb.Save();
                return Json(new { Status = true, Message = "Success" });
            }
            catch
            {
                return Json(new { Status = false, Message = "Fail" });
            }

        }

        public ActionResult ChangeHideStatusforAll(int project_ID, bool status)
        {
            try
            {
                var resourcelist = resourcedb.FindAllResources(project_ID);
                foreach (var r in resourcelist)
                {
                    r.Hide = status;
                }
                resourcedb.Save();
                return Json(new { Status = true, Message = "Success" });
            }
            catch (Exception ex)
            {
                return Json(new { Status = false, Message = "Fail" + ex.Message });
            }

        }


        public ActionResult ChangeHideStatusForUser(int resource_ID, bool status)
        {
            try
            {
                var resource = resourcedb.GetResource(resource_ID);
                resource.Hide = status;
                resourcedb.Save();
                return Json(new { Status = true, Message = "Success" });
            }
            catch(Exception ex)
            {
                return Json(new { Status = false, Message = "Fail " + ex.Message  });
            }

        }


        public ActionResult ChangeActiveStatusforAll(int project_ID, bool status)
        {
            try
            {
                var resourcelist = resourcedb.FindAllResources(project_ID);
                foreach (var r in resourcelist)
                {
                    r.active = status;
                }
                resourcedb.Save();
                return Json(new { Status = true, Message = "Success" });
            }
            catch
            {
                return Json(new { Status = false, Message = "Fail" });
            }

        }


        public ActionResult ChangeActiveStatusForUser(int resource_ID, bool status)
        {
            try
            {
                var resource = resourcedb.GetResource(resource_ID);
                resource.active = status;
                resourcedb.Save();
                return Json(new { Status = true, Message = "Success" });
            }
            catch
            {
                return Json(new { Status = false, Message = "Fail" });
            }

        }


    }
}
