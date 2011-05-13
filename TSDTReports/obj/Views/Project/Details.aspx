<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<TSDTReports.Models.tbl_Project>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Details
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Details</h2>
        <div class="pagerBox">    
    <div style="float:left">
        <a href="<%=HttpRuntime.AppDomainAppVirtualPath.TrimEnd('/') %>/Project/Create"><img src="<%=HttpRuntime.AppDomainAppVirtualPath.TrimEnd('/') %>/images/new-16x16.png" title="New" alt="New" /></a>
        <a href="<%=HttpRuntime.AppDomainAppVirtualPath.TrimEnd('/') %>/Project/Edit/<%= Html.Encode(Model.Project_ID) %>"><img src="<%=HttpRuntime.AppDomainAppVirtualPath.TrimEnd('/') %>/images/edit-16x16.png" title="Edit" alt="Edit" /></a>
        <a href="<%=HttpRuntime.AppDomainAppVirtualPath.TrimEnd('/') %>/Project/Index"><img src="<%=HttpRuntime.AppDomainAppVirtualPath.TrimEnd('/') %>/images/list-16x16.png" title="Back to List" alt="Back to List" /></a>
        <a href="<%=HttpRuntime.AppDomainAppVirtualPath.TrimEnd('/') %>/Project/Delete/<%= Html.Encode(Model.Project_ID) %>"><img src="<%=HttpRuntime.AppDomainAppVirtualPath.TrimEnd('/') %>/images/delete-16x16.png" title="Delete" alt="Delete" /></a>
        </div>
    </div>

    <span>
        <%=ViewData["Error"] %></span>
    <table class="Control" style="border-collapse: collapse;" border="0" cellspacing="0">
        <tr>
            <td>
                <table class="inputtable" cellspacing="0" cellpadding="3">
                    <tr>
                        <td class="inputlabel">Project ID</td>
                        <td class="inputdata">
                            <%= Html.Encode(Model.Project_ID) %>
                        </td>
                    </tr>
                    <tr>
                        <td class="inputlabel">Project Number</td>
                        <td class="inputdata">
                            <%= Html.Encode(Model.Project_Number) %></td>
                    </tr>
                    <tr>
                        <td class="inputlabel">SAP Number</td>
                        <td class="inputdata">
                            <%= Html.Encode(Model.SAP_Number) %></td>
                    </tr>
                    <tr>
                        <td class="inputlabel">WBS</td>
                        <td class="inputdata">
                            <%= Html.Encode(Model.WBS) %></td>
                    </tr>
                    <tr>
                        <td class="inputlabel">Project Name</td>
                        <td class="inputdata"><%= Html.Encode(Model.Project_Name) %></td>
                    </tr>
                    <tr>
                        <td class="inputlabel">Status</td>
                        <td class="inputdata"><%= Html.Encode(Model.tbl_Status.Status) %></td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="headContent" runat="server">
</asp:Content>
