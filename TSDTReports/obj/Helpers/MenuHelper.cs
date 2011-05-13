using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Text;
using System.Web.Mvc;
using TSDTReports.Models;

namespace TSDTReports.Helpers
{
    public static class MenuHelper
    {

        private static void LoopBranch(SiteMapNodeCollection nodeCollection, ref StringBuilder sb, ref HtmlHelper helper)
        {        
            IRoleRepository db = new RoleRepository();
            string User_CAI = HttpContext.Current.User.Identity.Name.ToUpper().Replace("CT\\","");
            string User_Role = db.GetUserRolesByUser(User_CAI);
            foreach (SiteMapNode node in nodeCollection)
            {
                string newUrl = HttpRuntime.AppDomainAppVirtualPath.TrimEnd('/');
                bool nodeDisplayStatus = false;
                foreach (var role in node.Roles)
                {
                    if (role.Equals("*") || role.Equals(User_Role))
                    {
                        nodeDisplayStatus = true;
                        break;
                    }
                }

                if (nodeDisplayStatus)
                {
                    sb.AppendLine("<li>");

                    if (SiteMap.CurrentNode == node)
                    {
                        newUrl += node.Url;
                        if (node.ChildNodes.Count > 0)
                            sb.AppendFormat("<a class='selectedMenuItem' href='{0}'>{1}</a>", newUrl, helper.Encode(node.Title));
                        else
                            sb.AppendFormat("<a class='selectedMenuItem' href='{0}'>{1}</a>", newUrl, helper.Encode(node.Title));
                    }
                    else
                    {
                        newUrl += node.Url;
                        if (node.ChildNodes.Count > 0)
                            sb.AppendFormat("<a href='{0}'>{1}</a>", newUrl, helper.Encode(node.Title));
                        else
                            sb.AppendFormat("<a href='{0}'>{1}</a>", newUrl, helper.Encode(node.Title));

                    }

                    if (node.ChildNodes.Count > 0)
                    {
                        sb.Append("<ul>");
                        LoopBranch(node.ChildNodes, ref sb, ref helper);
                        sb.Append("</ul>");
                    }
                    sb.AppendLine("</li>");
                }
            }



        }


        public static string Menu(this HtmlHelper helper)
        {
            var sb = new StringBuilder();            
            var topLevelNodes = SiteMap.RootNode.ChildNodes;

            try
            {
            //Create opening unordered list tag 


            sb.Append("<ul id='nav'>");

            LoopBranch(topLevelNodes, ref sb, ref helper);

            sb.Append("</ul>");

            return sb.ToString();
            }
            catch
            {
                //Create opening unordered list tag 

                sb.Append("Access Denied!! Please speak to Administrator of TSD Timesheets");

                return sb.ToString();
            }


        }
    }
}
