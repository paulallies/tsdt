using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace TSDTReports
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {

        protected void Application_BeginRequest()
        {
            HttpContext.Current.Response.Cache.SetExpires(DateTime.UtcNow.AddDays(-1));
            HttpContext.Current.Response.Cache.SetValidUntilExpires(false); 
            HttpContext.Current.Response.Cache.SetRevalidation(HttpCacheRevalidation.AllCaches);
            HttpContext.Current.Response.Cache.SetCacheability(HttpCacheability.NoCache);
            HttpContext.Current.Response.Cache.SetNoStore();
        }


        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "UserRoute",
                 "User/{action}/{User_CAI}", // URL with parameters
                 new { controller = "User", action = "Index" }  // Parameter defaults

                );
            routes.MapRoute(
                "ProjectRoute",
                 "Project/{action}/{Project_ID}", // URL with parameters
                 new { controller = "Project", action = "Index" }  // Parameter defaults

                );
            routes.MapRoute("ProjectOwnerDelete","ProjectOwner/Delete/Project/{Project_ID}/User_CAI/{User_CAI}",new { controller = "ProjectOwner", action = "Delete" });
            routes.MapRoute("ProjectOwner", "ProjectOwner/ProjectList/{User_CAI}", new { controller = "ProjectOwner", action = "ProjectList" });
            routes.MapRoute("ProjectOwnerGetGrid", "ProjectOwner/GridData/{User_CAI}", new { controller = "ProjectOwner", action="GridData" });
            routes.MapRoute("ProjectResource", "MyProject/GetProjectResources/{Project_ID}", new { controller = "MyProject", action = "GetProjectResources" });
            routes.MapRoute("TimeSheet", "TimeSheet/Edit/{id}/Date/{myDate}/User/{User_CAI}", new { controller = "TimeSheet", action = "Edit" });
            routes.MapRoute("TimeSheetCancel", "TimeSheet/Cancel/Date/{myDate}/User/{User_CAI}", new { controller = "TimeSheet", action = "Cancel" });


            routes.MapRoute(
                "UserPageRoute",
                "{controller}/{action}/Page/{Page}/size/{size}/Search/{Search}", // URL with parameters
                new { controller = "User", action = "Search", Search = "" }  // Parameter defaults

                );

            routes.MapRoute(
                "ProjectPageRoute",
                 "{controller}/{action}/Page/{Page}/size/{size}/Search/{Search}", // URL with parameters
                 new { controller = "Project", action = "Search", Search = "" }  // Parameter defaults

                );

            routes.MapRoute(
                "Default",                                              // Route name
                "{controller}/{action}/{id}",
                new { controller = "Timesheet", action = "Index", id = "" }  // Parameter defaults


                );



        }

        protected void Application_Start()
        {
            RegisterRoutes(RouteTable.Routes);
        }
    }
}