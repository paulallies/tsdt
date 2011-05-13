<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<%
    if (Request.IsAuthenticated) {
        try
        {%>

        User: <b><%= Html.Encode(Page.User.Identity.Name.ToUpper().Replace("CT\\", ""))%></b>
        Role: <b><%=Html.Encode(new TSDTReports.Models.RoleRepository().GetUserRolesByUser(Page.User.Identity.Name.ToUpper().Replace("CT\\", "")))%></b>
      
<%}
        catch
        {%>
        User : <b><%= Html.Encode(Page.User.Identity.Name.ToUpper().Replace("CT\\", ""))%></b>
        Logged In..
<%}}%> 

