<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<TSDTReports.Models.tbl_Project>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Create
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Create</h2>

    <%= Html.ValidationSummary("Create was unsuccessful. Please correct the errors and try again.") %>

    <% using (Html.BeginForm()) {%>
       <div class="pagerBox">    
    <div style="float:left">

        <a href="<%=HttpRuntime.AppDomainAppVirtualPath.TrimEnd('/') %>/Project/Index"><img src="<%=HttpRuntime.AppDomainAppVirtualPath.TrimEnd('/') %>/images/list-16x16.png" title="Back to List" alt="Back to List" /></a>
        
        </div>
    </div>
<table class="Control" style="border-collapse: collapse;" border ="0" cellspacing="0">
<tr>
<td>
        <table class="inputtable" cellspacing="0" cellpadding="3" >
            <tr>
                <td class="inputlabel">Project Number:</td>
                <td class="inputdata"><%= Html.TextBox("Project_Number")%>
                <%= Html.ValidationMessage("Project_Number", "*")%></td>
            </tr>
            <tr>
                <td class="inputlabel">SAP Number</td>
                <td class="inputdata"><%= Html.TextBox("SAP_Number")%>
                <%= Html.ValidationMessage("SAP_Number", "*")%></td>
            </tr>
            <tr>
                <td class="inputlabel">WBS:</td>
                <td class="inputdata"><%= Html.TextBox("WBS") %>
                <%= Html.ValidationMessage("WBS", "*") %></td>
            </tr>
            <tr>
                <td class="inputlabel">Project Name:</td>
                <td class="inputdata"><%= Html.TextBox("Project_Name") %>
                <%= Html.ValidationMessage("Project_Name", "*") %></td>
            </tr>
            
             <tr>
                <td class="inputlabel">Status:</td>
                <td class="inputdata"><%= Html.DropDownList("Status_ID", (SelectList)ViewData["Status"], "Select")%>
                <%= Html.ValidationMessage("Status_ID", "*") %></td>
            </tr>
            <tr>
            <td class="inputlabel"></td>
            <td class="inputdata">            
            <a href="javascript:$('form').submit();"><img src="<%=HttpRuntime.AppDomainAppVirtualPath.TrimEnd('/') %>/images/check-16x16.png" /></a>
            <a href="<%=HttpRuntime.AppDomainAppVirtualPath.TrimEnd('/') %>/Project/Index" ><img src="<%=HttpRuntime.AppDomainAppVirtualPath.TrimEnd('/') %>/images/undo-16x16.png" title="Cancel" alt="cancel" /></a></td>
            </tr>
        </table>
        </td>
        </tr>
        </table>

    <% } %>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="headContent" runat="server">
</asp:Content>

