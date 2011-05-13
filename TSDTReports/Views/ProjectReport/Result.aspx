<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<TSDTReports.Models.UserData>>" %>

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
        <legend>Project Details</legend>
        <ul>
            <li>
                <label>MOC:</label>
                <%=Html.Encode(((TSDTReports.Models.tbl_Project)ViewData["Project"]).Project_Number) %>
            </li>
            <li>
                <label>Name:</label>
                <%=Html.Encode(((TSDTReports.Models.tbl_Project)ViewData["Project"]).Project_Name)%>
            </li>
            <li>
                <label>Status:</label>
                <%=Html.Encode(((TSDTReports.Models.tbl_Project)ViewData["Project"]).tbl_Status.Status)%>
            </li>
            <li>
                <label>WBS:</label>
                <%=Html.Encode(((TSDTReports.Models.tbl_Project)ViewData["Project"]).WBS)%>
            </li>
            <li>
                <label>SAP Number:</label>
                <%=Html.Encode(((TSDTReports.Models.tbl_Project)ViewData["Project"]).SAP_Number)%>
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
                <%-- <th></th>--%>
                <th>First Name </th>
                <th>Last Name </th>
                <th>CAI </th>
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
            <td>
                <%= Html.Encode(item.FirstName) %>
            </td>
            <td>
                <%= Html.Encode(item.LastName) %>
            </td>
            <td>
                <%= Html.Encode(item.CAI) %>
            </td>
            <td style="text-align: right">
                <%= Html.Encode(String.Format("{0:F}", item.hours)) %>
            </td>
        </tr>
        <% total += item.hours; %>
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
