<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<TSDTReports.Models.tbl_Project>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Edit
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h2>Edit</h2>
    <%= Html.ValidationSummary("Edit was unsuccessful. Please correct the errors and try again.") %>
    <% using (Html.BeginForm())
       {%>
    <table class="Control" style="border-collapse: collapse;" border="0" cellspacing="0">
        <tr>
            <td>
                <table class="inputtable" cellspacing="0" cellpadding="3">
                    <tr>
                        <td class="inputlabel">Project ID</td>
                        <td class="inputdata"><%=Model.Project_ID %></td>
                    </tr>
                    <tr>
                        <td class="inputlabel">Project Number</td>
                        <td class="inputdata">
                            <%= Html.TextBox("Project_Number", Model.Project_Number) %>
                            <%= Html.ValidationMessage("Project_Number", "*") %>
                        </td>
                    </tr>
                    <tr>
                        <td class="inputlabel">SAP_Number</td>
                        <td class="inputdata">
                            <%= Html.TextBox("SAP_Number", Model.SAP_Number) %>
                            <%= Html.ValidationMessage("SAP_Number", "*") %>
                        </td>
                    </tr>
                    <tr>
                        <td class="inputlabel">WBS</td>
                        <td class="inputdata">
                            <%= Html.TextBox("WBS", Model.WBS) %>
                            <%= Html.ValidationMessage("WBS", "*") %>
                        </td>
                    </tr>
                    <tr>
                        <td class="inputlabel">Project_Name</td>
                        <td class="inputdata">
                            <%= Html.TextBox("Project_Name", Model.Project_Name) %>
                            <%= Html.ValidationMessage("Project_Name", "*") %>
                        </td>
                    </tr>
                    <tr>
                        <td class="inputlabel">Status</td>
                        <td class="inputdata">
                            <%= Html.DropDownList("Status_ID", (SelectList)ViewData["Status"])%></td>
                    </tr>
                    <tr>
                        <td class="inputlabel"></td>
                        <td class="inputdata"><a href="javascript:$('form').submit();">
                            <img src="<%=HttpRuntime.AppDomainAppVirtualPath.TrimEnd('/') %>/images/check-16x16.png" /></a> <a href="<%=HttpRuntime.AppDomainAppVirtualPath.TrimEnd('/') %>/Project/Details/<%=Model.Project_ID %>">
                                <img src="<%=HttpRuntime.AppDomainAppVirtualPath.TrimEnd('/') %>/images/undo-16x16.png" title="Cancel" alt="cancel" /></a> </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>

    <% } %>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="headContent" runat="server">
</asp:Content>
