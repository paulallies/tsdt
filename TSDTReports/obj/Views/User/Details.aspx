<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<TSDTReports.Models.tbl_User>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Details
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h2>Details</h2>
    <div class="pagerBox">
        <div style="float: left">
            <a href="<%=HttpRuntime.AppDomainAppVirtualPath.TrimEnd('/') %>/User/Create"><img src="<%=HttpRuntime.AppDomainAppVirtualPath.TrimEnd('/') %>/images/new-16x16.png" title="New" alt="New" /></a> 
            <a href="<%=HttpRuntime.AppDomainAppVirtualPath.TrimEnd('/') %>/User/Edit/<%= Html.Encode(Model.User_CAI) %>"><img src="<%=HttpRuntime.AppDomainAppVirtualPath.TrimEnd('/') %>/images/edit-16x16.png" title="Edit" alt="Edit" /></a> 
            <a href="<%=HttpRuntime.AppDomainAppVirtualPath.TrimEnd('/') %>/User/Index"><img src="<%=HttpRuntime.AppDomainAppVirtualPath.TrimEnd('/') %>/images/list-16x16.png" title="Back to List" alt="Back to List" /></a>
            <a href="<%=HttpRuntime.AppDomainAppVirtualPath.TrimEnd('/') %>/User/Delete/<%= Html.Encode(Model.User_CAI) %>"><img src="<%=HttpRuntime.AppDomainAppVirtualPath.TrimEnd('/') %>/images/delete-16x16.png" title="Delete" alt="Delete" /></a>
        </div>
    </div>
    <span>
        <%=ViewData["Error"] %></span>
    <table class="Control" style="border-collapse: collapse;" border="0" cellspacing="0">
        <tr>
            <td>
                <table class="inputtable" cellspacing="0" cellpadding="3">
                    <tr>
                        <td class="inputlabel">User CAI:</td>
                        <td class="inputdata">
                            <%= Html.Encode(Model.User_CAI) %></td>
                    </tr>
                    <tr>
                        <td class="inputlabel">User First Name:</td>
                        <td class="inputdata">
                            <%= Html.Encode(Model.User_FirstName) %></td>
                    </tr>
                    <tr>
                        <td class="inputlabel">User Last Name:</td>
                        <td class="inputdata">
                            <%= Html.Encode(Model.User_LastName) %></td>
                    </tr>
                    <tr>
                        <td class="inputlabel">Employee Number:</td>
                        <td class="inputdata"><span id="EmployeeNumber"><%= Html.Encode(Model.EmployeeNumber)%></span></td>
                    </tr>
                    <tr>
                        <td class="inputlabel">Company Name:</td>
                        <td class="inputdata">
                            <%= Html.Encode(Model.CompanyName)%></td>
                    </tr>
                    <tr>
                        <td class="inputlabel">Company ID:</td>
                        <td class="inputdata"> <%= Html.Encode(Model.CompanyID)%></td>
                    </tr>
                    <tr>
                        <td class="inputlabel">User Role:</td>
                        <td class="inputdata">
                            <%= Html.Encode(Model.User_Role) %></td>
                    </tr>
                    <tr>
                        <td class="inputlabel">Acitve:</td>
                        <td class="inputdata">
                            <%= Html.Encode(Model.active) %></td>
                    </tr>
                    <tr>
                        <td class="inputlabel">Photo:</td>
                        <td class="inputdata"><img src="" id="imgPhoto" title="" alt="" /></td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="headContent" runat="server">
    <script type="text/javascript">


        $(function() {

            $("#imgPhoto").attr("src", "http://zarfweb.chevron.com/SACO2/photo.ashx?id=" + $("#EmployeeNumber").text());

        });
   
    </script>
</asp:Content>
