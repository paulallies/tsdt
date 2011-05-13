<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<TSDTReports.Models.tbl_User>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Create
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Create</h2>

    <%= Html.ValidationSummary("Create was unsuccessful. Please correct the errors and try again.") %>

    <% using (Html.BeginForm()) {%>
   <div class="pagerBox">    
    <div style="float:left">

        <a href="<%=HttpRuntime.AppDomainAppVirtualPath.TrimEnd('/') %>/User/Index"><img src="<%=HttpRuntime.AppDomainAppVirtualPath.TrimEnd('/') %>/images/list-16x16.png" title="Back to List" alt="Back to List" /></a>
        
        </div>
    </div>
<table class="Control" style="border-collapse: collapse;" border ="0" cellspacing="0">
<tr>
<td>
        <table class="inputtable" cellspacing="0" cellpadding="3" >
            <tr>
                <td class="inputlabel">User CAI:</td>
                <td class="inputdata"><%= Html.TextBox("User_CAI") %>
                <%= Html.ValidationMessage("User_CAI", "*") %></td>
            </tr>
            <tr>
                <td class="inputlabel">User First Name:</td>
                <td class="inputdata"><%= Html.TextBox("User_FirstName") %>
                <%= Html.ValidationMessage("User_FirstName", "*") %></td>
            </tr>
            <tr>
                <td class="inputlabel">User Last Name:</td>
                <td class="inputdata"><%= Html.TextBox("User_LastName") %>
                <%= Html.ValidationMessage("User_LastName", "*") %></td>
            </tr>
            <tr>
                <td class="inputlabel">User Role:</td>
                <td class="inputdata"><%= Html.DropDownList("User_Role", (SelectList)ViewData["Roles"])%></td>
            </tr>
            <tr>
                <td class="inputlabel">Active:</td>
                <td class="inputdata"><%= Html.CheckBox("Active", (SelectList)ViewData["Active"])%></td>
            </tr>
            <tr>
            <td class="inputlabel"></td>
            <td class="inputdata">            
            <a href="javascript:$('form').submit();"><img src="<%=HttpRuntime.AppDomainAppVirtualPath.TrimEnd('/') %>/images/check-16x16.png" /></a>
            <a href="<%=HttpRuntime.AppDomainAppVirtualPath.TrimEnd('/') %>/User/Index" ><img src="<%=HttpRuntime.AppDomainAppVirtualPath.TrimEnd('/') %>/images/undo-16x16.png" title="Cancel" alt="cancel" /></a></td>
            </tr>
        </table>
        </td>
        </tr>
        </table>
    <% } %>



</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="headContent" runat="server">
</asp:Content>

