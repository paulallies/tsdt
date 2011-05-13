<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    <%=Html.ViewData["Title"] %>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="headContent" runat="server">

    <script type="text/javascript">
        $(function() {
            $('input[id$="Date"]').datepicker({ dateFormat: 'dd-M-yy', duration: 'fast' });

            $("#btnGetReport").click(function() {
                if (
                $("#ddlUsers").val() == 0 ||
               $('#txtStartDate').val() == "" ||
               $('#txtEndDate').val() == "") {
                    alert("Errors");
                    return false;
                }
                else return true;
            });
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
    <% using (Html.BeginForm())
       { %>
    <h2>
        <%=Html.ViewData["Title"]%></h2>
    <fieldset>
        <legend>User Details</legend>
        <ul>
            <li>
                <label>User</label>
                <%=Html.DropDownList("ddlUsers", (SelectList)ViewData["Users"], "Select")%>
            </li>
            <li>
                <label>Start Date</label>
                <%= Html.TextBox("txtStartDate", ViewData["startdate"], new { autocomplete = "off" })%>
            </li>
            <li>
                <label>End Date</label>
                <%= Html.TextBox("txtEndDate", ViewData["enddate"], new { autocomplete = "off" })%>
            </li>
            <li>
                <label></label>
                <input type="submit" value="Get User Report" name="submitbutton" id="btnGetReport" />
            </li>
        </ul>
    </fieldset>

    <%
        var list = (List<TSDTReports.Models.ProjectHours>)ViewData["ProjectHours"];
        if (ViewData["ProjectHours"] != null)
        {
          if (list.Count() == 0)
          {%>
    
    <table id="Table2" class="tablesorter" style="width: 100%">
        <thead>
            <tr>
                <th>No Data</th>
            </tr>
        </thead>
    </table>
    
     <%}
      else
      { %>
    
    <input type="submit" value="Print" name="submitbutton" />
    <table id="dataTable" class="tablesorter">
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
        <% if (ViewData["ProjectHours"] != null)
           { %>
        <% foreach (var item in (List<TSDTReports.Models.ProjectHours>)ViewData["ProjectHours"])
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
                <%= Html.Encode(item.ProjectNumber)%>
            </td>
            <td>
                <%= Html.Encode(item.ProjectName)%>
            </td>
            <td>
                <%= Html.Encode(item.ProjectStatus)%>
            </td>
            <td style="text-align: right">
                <%= Html.Encode(String.Format("{0:F}", item.Hours))%>
            </td>
        </tr>
        <% total += item.Hours; %>
        <% } %>
        <%} %>
        <tfoot>
            <tr>
                <th colspan="3">Total</th>
                <th style="text-align: right">
                    <% = Html.Encode(String.Format("{0:F}", total))%></th>
            </tr>
        </tfoot>
    </table>
    
    <%} %>
    <%} %>
        <%} %>
</asp:Content>
