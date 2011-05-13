<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>
<%@ Import Namespace="TSDTReports.Helpers"%>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	ProjectOwners
</asp:Content>


<asp:Content ID="Content3" ContentPlaceHolderID="headContent" runat="server">
</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>My Projects</h2>
    
   <%var list = (List<TSDTReports.Models.tbl_Project>)ViewData["OwnedProjects"];
    %>
    <%if (list == null || list.Count == 0)
      { }
      else
      { %>

    <table class="Control" border="1" cellpadding="3" cellspacing="0">
        <tr class="HeaderStyle">
            <th></th>

            <th>
                Project Number
            </th>
            <th>
                SAP Number
            </th>
            <th>
                WBS
            </th>
            <th>
                Project Name
            </th>
            <th>
                Status ID
            </th>
        </tr>

    <% foreach (var item in list)
       { %>
    
        <tr>
            <td>
                <a href="/Project/Edit/<%=item.Project_ID %>"><img src="../../images/icon_open.jpg" alt="Details" title="Details" /></a>
            </td>

            <td>
                <%= Html.Encode(item.Project_Number)%>
            </td>
            <td>
                <%= Html.Encode(item.SAP_Number)%>
            </td>
            <td>
                <%= Html.Encode(item.WBS)%>
            </td>
            <td>
                <%= Html.Encode(item.Project_Name)%>
            </td>
            <td>
                <%= Html.Encode(item.tbl_Status.Status)%>
            </td>
        </tr>
    
    <% } %>

    </table>
		

     <%} %>
</asp:Content>