<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    <%=Html.ViewData["Title"] %>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="headContent" runat="server">

    <script type="text/javascript">
        $().ready(function() {
            $('input[id$=Date]').datepicker({ dateFormat: 'dd-M-yy', duration: 'fast' });

            $('input[id=checkall]').click(function() {
                if ($(this).attr("checked") == true) {
                    $('input[id^=chk]').attr("checked", true);
                }
                else {
                    $('input[id^=chk]').attr("checked", false);
                }

            });

            $('input[id^=chk]').click(function() {
                if ($(this).attr("checked") == false) {
                    $('input[id=checkall]').attr("checked", false);
                }
            });



            //We must lastly verify on the submit click
            $('input[id=btnGetProjectReport]').click(function() {
                if (
                verifyDateElement('input[id=txtStartDate]', 'sdv') == false ||
                verifyDateElement('input[id=txtEndDate]', 'edv') == false
               )
                    return false;
                else
                    return true;

            });

            $('#btnFind').click(function() {
            if ($.trim($("#txtSearch").val()).length == 0) {
                    alert("No Search Criteria...");
                    return false;
                }
                else {
                    return true;
                }

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
    <h2>
        <%=Html.ViewData["Title"] %></h2>
    <% using (Html.BeginForm())
       { %>
       <div style="float: left">
        Find Project:
          <%=Html.DropDownList("ddlCols", (SelectList)ViewData["Cols"], new { @class = "search", @style="width:120px" })%>
          <%=Html.DropDownList("ddlOper", (SelectList)ViewData["Oper"], new { @class = "search" })%>
            <%=Html.TextBox("txtSearch", ViewData["Search"], new {autocomplete="off", @class="search" })%>
            <input type="submit" id="btnFind" value="Find" class="buttonsearch" name="submitbutton"/>

        </div>
        <br/>
    <%--      
    <label>Project Number:</label>
    <%=Html.TextBox("txtProjectNumber", ViewData["ProjectNumber"], new {autocomplete="off" })%>
    <input type="submit" value="Search" id="btnProjectSearch" name="submitbutton" />
    <br />--%>
    
    <%var list = (List<TSDTReports.Models.tbl_Project>)ViewData["Projects"]; %>
    <%if (list == null || list.Count == 0)
      { }
      else
      { %>

    <table id="dataTable" class="tablesorter" style="width: 100%">
        <tr>
            <th>Project Number</th>
            <th>SAP Number</th>
            <th>WBS</th>
            <th>Project Name</th>
            <th>Status</th>
            <th>Select<%=Html.CheckBox("checkall",(ViewData["checkall"] == null)? false: bool.Parse(ViewData["checkall"].ToString()))%>
            </th>
        </tr>
        <% foreach (var item in (List<TSDTReports.Models.tbl_Project>)ViewData["Projects"])
           { %>
        <tr>
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
            <td>
                <%=Html.CheckBox("chk" + item.Project_ID,(ViewData["chk" + item.Project_ID.ToString()] == null)? false: bool.Parse(ViewData["chk" + item.Project_ID.ToString()].ToString()))%>
            </td>
        </tr>
        <% } %>
    </table>
    <br />
    <ul>
        <li>
            <label>Start Date</label>
            <%= Html.TextBox("txtStartDate", ViewData["startdate"],   new { autocomplete = "off"})%>
        </li>
        <li>
            <label>End Date</label>
            <%= Html.TextBox("txtEndDate", ViewData["enddate"], new { autocomplete = "off" })%>
        </li>
        <li>
            <label></label>
            <input type="submit" value="Get Project Report" id="btnGetProjectReport" name="submitbutton" />
            <span><%=Html.ViewData["Message"] %></span>
        </li>
        </ul>

    <%} %>

    <% 
        var UserDataList = (List<TSDTReports.Models.UserData>)ViewData["userDataCollection"];
        if (ViewData["userDataCollection"] != null)
        {
            if (UserDataList.Count() == 0)
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
            <input type="submit" name="submitbutton" value="Print" />
    <table id="Table1" class="tablesorter" style="width: 100%">
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
        <% foreach (var item in (List<TSDTReports.Models.UserData>)ViewData["UserDataCollection"])
           { %>
        <tr>
            <%--            <td>
                <%= Html.ActionLink("Edit", "Edit", new { /* id=item.PrimaryKey */ }) %> |
                <%= Html.ActionLink("Details", "Details", new { /* id=item.PrimaryKey */ })%>
            </td>--%>
            <td>
                <%= Html.Encode(item.FirstName)%>
            </td>
            <td>
                <%= Html.Encode(item.LastName)%>
            </td>
            <td>
                <%= Html.Encode(item.CAI)%>
            </td>
            <td style="text-align: right">
                <%= Html.Encode(String.Format("{0:F}", item.hours))%>
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
    <%} %>
    <%} %>
        <%} %>
</asp:Content>
