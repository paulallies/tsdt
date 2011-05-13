<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<TSDTReports.Models.ProjectHours>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    <%=Html.ViewData["Title"] %>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="headContent" runat="server">

    <script type="text/javascript">
        $().ready(function() {
            $("#dataTable").tablesorter();
        });

    </script>

    <style type="text/css">
        ul li
        {
            list-style: none;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h2>
        <%=Html.ViewData["Title"] %></h2>
    <fieldset style="width:97%">
        <legend>User Details</legend>
        <ul>
            <li>
                <label>Name:</label>
                <%=Html.Encode(((TSDTReports.Models.tbl_User)ViewData["User"]).User_FirstName + " " + ((TSDTReports.Models.tbl_User)ViewData["User"]).User_LastName)%>
            </li>
            <li>
                <label>CAI:</label>
                <%=Html.Encode(((TSDTReports.Models.tbl_User)ViewData["User"]).User_CAI)%>
            </li>
            <li>
                <label>Date Period:</label>
                <%=Html.ViewData["StartDate"] %>&nbsp;to&nbsp;<%=Html.ViewData["EndDate"] %>
            </li>
        </ul>
    </fieldset>
    <table id="dataTable" class="tablesorter" style="width:98%">
        <thead>
            <tr>
                <%--   <th></th>--%>
                <%--    <th>ProjectID </th>--%>
                <th>Project Number </th>
                <th>Project Name </th>
                <th>Project Status </th>
                <th>Hours </th>
            </tr>
        </thead>
        <%decimal? total = 0; %>
        <% foreach (var item in Model)
           { %>
        <tr>
            <%--            <td>
                <%= Html.ActionLink("Edit", "Edit", new { /* id=item.PrimaryKey */ }) %> |
                <%= Html.ActionLink("Details", "Details", new { /* id=item.PrimaryKey */ })%>
            </td>--%>
            <%--            <td>
                <%= Html.Encode(item.ProjectID) %>
            </td>--%>
            <td>
                <%= Html.Encode(item.ProjectNumber) %>
            </td>
            <td>
                <%= Html.Encode(item.ProjectName) %>
            </td>
            <td>
                <%= Html.Encode(item.ProjectStatus) %>
            </td>
            <td style="text-align: right">
                <%= Html.Encode(String.Format("{0:F}", item.Hours)) %>
            </td>
        </tr>
        <% total += item.Hours; %>
        <% } %>
        <tfoot>
            <tr>
                <th colspan="3">Total</th>
                <th style="text-align: right">
                    <% = Html.Encode(String.Format("{0:F}", total))%></th>
            </tr>
        </tfoot>
    </table>
    <%--    <p>
        <%= Html.ActionLink("Create New", "Create") %>
    </p>--%>
</asp:Content>
