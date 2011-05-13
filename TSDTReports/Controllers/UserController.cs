using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using TSDTReports.Models;

namespace TSDTReports.Controllers
{
    public class UserController : Controller
    {
        IUserRepository db = new UserRepository();
        IRoleRepository roledb = new RoleRepository();
        IProjectRepository projdb = new ProjectRepository();
        ISacoRepository sacodb = new SacoRepository();

        public ActionResult Index(string Search, int? page, int? size)
        {
            ViewData["Title"] = "User List";
            ViewData["Search"] = Search;
            ViewData["Title"] = "Project List";
            string role = roledb.GetUserRolesByUser(User.Identity.Name.ToString().ToUpper().Replace("CT\\",""))[0];
            if (!role.Equals("Administrator"))
                return View("AccessDenied");
            else
                return View();
        }

        public ActionResult GetWhitePagesDetails(string cai)
        {
            com.chevron.ctpeople.getpeople ws = new TSDTReports.com.chevron.ctpeople.getpeople();
            List<string> subList = new List<string>();
            try
            {
                var test = ws.dsGetPersonByCAI(cai).Tables[0].Rows[0];
                var jsondata = new 
                {
                    WhitePagesDetails = 
                    (
                        new 
                        {
                           firstname = test.ItemArray[6],
                           surname = test.ItemArray[8]
                        }
                    )
                };
                return Json(jsondata);
                
            }
            catch
            {
                return Json(new { Status = false, Message = "Fail" });
            }
        }

        public ActionResult GetUserEmployees(string cai)
        {
            string Surname = "";
            try
            {
                com.chevron.ctpeople.getpeople ws = new TSDTReports.com.chevron.ctpeople.getpeople();
                var test = ws.dsGetPersonByCAI(cai).Tables[0].Rows[0];
                Surname = test.ItemArray[8].ToString();
            }
            catch
            {
                return Json(new { Status = false, Message = "This user does not exist in WhitePages" });
            }
            try
            {
                var jsondata = new
                {
                    employees = 
                    (
                        from e in sacodb.EmployeeList(Surname.Trim())
                        select new 
                        {
                            employee_number = e.EmployeeNumber,
                            employee_surname = e.Surname,
                            employee_firstname =  e.FirstName,
                            employee_company = e.Company.Name,
                            employee_companyid = e.CompanyId
                        }
                    )
                };
                return Json(jsondata);
            }
            catch (Exception ex)
            {
                return Json(new { Status = false, Message = "This user does not exist in Saco" });
            }
        }

       

        public ActionResult GetUsers(string sidx, string sord, int page, int rows, bool _search, string searchField, string searchOper, string searchString)
        {
            int pageIndex = Convert.ToInt32(page) - 1;
            int pageSize = rows;
            int totalRecords = db.GetUsers().Count();
            if (_search)
            {
                totalRecords = db.GetSearchedUsers(searchField, searchString, searchOper).Count();
            }

            int totalPages = (int)Math.Ceiling((float)totalRecords / (float)pageSize);
            int currentpage = page;
            int defaultPageSize = rows;
            var pagedUsers = db.GetUsersPaged(pageIndex,defaultPageSize, sidx,sord);
            if (_search)
            {
                pagedUsers = db.GetSearchedUsersPaged(pageIndex, defaultPageSize, sidx, sord, searchField, searchString, searchOper);
            }

            var jsonData = new
            {
                total = totalPages,
                page = page,
                records = totalRecords,
                rows = (
                    from p in pagedUsers
                    select new
                    {
                        id = p.User_CAI,
                        cell = new string[]
                        {
                            p.User_CAI,
                            p.User_FirstName,
                            p.User_LastName,
                        }
                    }).ToArray()

            };
            return Json(jsonData);
        }

        public ActionResult GetAllUsers(string sidx, string sord, int page, int rows, bool _search, string searchField, string searchOper, string searchString)
        {
            int pageIndex = Convert.ToInt32(page) - 1;
            int pageSize = rows;
            int totalRecords = db.GetUsers().Count();
            if (_search)
            {
                totalRecords = db.GetSearchedUsers(searchField, searchString, searchOper).Count();
            }

            int totalPages = (int)Math.Ceiling((float)totalRecords / (float)pageSize);
            int currentpage = page;
            int defaultPageSize = rows;
            var pagedUsers = db.GetUsersPaged(pageIndex, defaultPageSize, sidx, sord);
            if (_search)
            {
                pagedUsers = db.GetSearchedUsersPaged(pageIndex, defaultPageSize, sidx, sord, searchField, searchString, searchOper);
            }

            var jsonData = new
            {
                total = totalPages,
                page = page,
                records = totalRecords,
                rows = (
                    from p in pagedUsers
                    select new
                    {
                        id = p.User_CAI,
                        cell = new string[]
                        {
                            "",
                            p.User_CAI,
                            p.User_FirstName,
                            p.User_LastName,
                            p.tbl_User_Role.User_Role_Descr,
                            p.active.ToString()
                        }
                    }).ToArray()

            };
            return Json(jsonData);
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Create()
        {
            ViewData["Roles"] = new SelectList(roledb.GetUserRoles(), "User_Role", "User_Role_Descr");
            return View();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Create(tbl_User myUser)
        {
            ViewData["Roles"] = new SelectList(roledb.GetUserRoles(), "User_Role", "User_Role_Descr");

            if (myUser.User_CAI.Trim().Length == 0)
                ModelState.AddModelError("User_CAI", "CAI is Required");

            if (myUser.User_FirstName.Trim().Length == 0)
                ModelState.AddModelError("User_FirstName", "First Name Required");

            if (myUser.User_LastName.Trim().Length == 0)
                ModelState.AddModelError("User_LastName", "Last Name Required");

            if (!ModelState.IsValid)
            {
                return View();
            }

            try
            {
                myUser.User_CAI = myUser.User_CAI.ToUpper().Trim();
                db.AddUser(myUser);
                db.Save();
                return RedirectToAction("Details", new { User_CAI = myUser.User_CAI, Error = "" });

            }
            catch(Exception ex)
            {
                ModelState.AddModelError("","Error: " + ex.Message); 
                return View();
            }
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Details(string User_CAI, string Error)
        {
            try
            {
                ViewData["Error"] = Error;
                return View(db.GetUserByID(User_CAI));
            }
            catch
            {
                return RedirectToAction("Index");
            }
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Edit(string User_CAI)
        {
            tbl_User myUser = db.GetUserByID(User_CAI);
            ViewData["Roles"] = new SelectList(roledb.GetUserRoles(), "User_Role", "User_Role_Descr", myUser.User_Role );
            ViewData["Active"] = myUser.active;
            return View(myUser);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Edit(tbl_User myUser)
        {
            string str = Request.Form[0];
            if (myUser.User_CAI.Trim().Length == 0)
                ModelState.AddModelError("User_CAI", "CAI is Required");

            if (myUser.User_FirstName.Trim().Length == 0)
                ModelState.AddModelError("User_FirstName", "First Name Required");

            if (myUser.User_LastName.Trim().Length == 0)
                ModelState.AddModelError("User_LastName", "Last Name Required");

            if (myUser.User_Role.Trim().Length == 0)
                ModelState.AddModelError("User_Role", "Role Required");

            if (!ModelState.IsValid)
            {
                ViewData["Roles"] = new SelectList(roledb.GetUserRoles(), "User_Role", "User_Role_Descr", myUser.User_Role);

                return View(myUser);
            }

            try
            {
                tbl_User dbUser = db.GetUserByID(myUser.User_CAI);
                dbUser.User_CAI = myUser.User_CAI;
                dbUser.User_FirstName = myUser.User_FirstName;
                dbUser.User_LastName = myUser.User_LastName;
                dbUser.EmployeeNumber = myUser.EmployeeNumber;
                dbUser.CompanyName = myUser.CompanyName;
                dbUser.CompanyID = myUser.CompanyID;
                dbUser.User_Role = myUser.User_Role;
                dbUser.active = myUser.active;
                db.Save();
                return RedirectToAction("Details",new {User_CAI = myUser.User_CAI});

            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Error: " + ex.Message);
                return View();
            }
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Delete(string User_CAI)
        {
            try
            {
                tbl_User dbUser = db.GetUserByID(User_CAI);
                db.DeleteUser(dbUser);
                db.Save();
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Error: " + ex.Message);

                return RedirectToAction("Details", new {User_CAI = User_CAI, Error = "Error deleting User!" });
            }
        }
    }
}
