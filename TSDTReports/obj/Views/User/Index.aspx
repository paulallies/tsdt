<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<%@ Import Namespace="TSDTReports.Helpers" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Index
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h2>User List</h2>
    <form id="frmSearch" method="post" action="/User/Index">
    <div class="pagerBox">
        <div style="float: left">
            <a href='<%= ResolveUrl("~/User/create") %>'>
                <img src='<%= ResolveUrl("~/images/new-16x16.png") %>' title="New" alt="New" /></a>
        </div>
    </div>
    <table id="User_List" class="scroll" cellpadding="0" cellspacing="0">
    </table>
    <div id="pagerUser_List" class="scroll" style="text-align: center;">
    </div>
    </form>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="headContent" runat="server">

    <script type="text/javascript">
        var urlPath = rtrim('<%=ResolveUrl("~/")%>', "/");
        $(document).ready(function() {
            $("#User_List").jqGrid({
            url: urlPath +  '/User/GetAllUsers/',
                datatype: 'json',
                mtype: 'GET',
                colNames: ['', 'CAI', 'First Name', 'Last Name', 'User Role', 'Active'],
                colModel: [
          { name: 'act', index: 'CAI', width: 50, align: 'left' },
          { name: 'User_CAI', index: 'User_CAI', width: 100, align: 'left' },
          { name: 'User_FirstName', index: 'User_FirstName', width: 100, align: 'left' },
          { name: 'User_LastName', index: 'User_LastName', width: 100, align: 'left' },
          { name: 'User_Role', index: 'User_Role', width: 100, align: 'left' },
          { name: 'Active', index: 'Active', width: 55, align: 'center', formatter: 'checkbox' },

          ],
                pager: jQuery('#pagerUser_List'),
                rowNum: 10,
                rowList: [5, 10, 20, 50],
                sortname: 'CAI',
                sortorder: "asc",
                viewrecords: true,
                imgpath: urlPath + '/scripts/themes/steel/images',
                caption: ' ',
                height: 290,
                loadComplete: function() {
                    var ids = $("#User_List").getDataIDs();
                    for (var i = 0; i < ids.length; i++) {
                        var cl = ids[i];
                        be = "<a href=" + urlPath + "/User/Details/" + cl + " ><img src='" + urlPath + "/images/Detail-16x16.png' title='Detail' alt='detail'/></a>";
                        $("#User_List").setRowData(ids[i], { act: be });
                    }
                }
            });



            $("table[class=Header] th").append('<div style="float: left">' +
        'Filter:' +
            '<select id="ddlField" class="search">' +
             '   <option value="User_CAI" selected="selected">CAI</option>' +
              '  <option value="User_FirstName">First Name</option>' +
               ' <option value="User_LastName">Last Name</option>' +
                '<option value="User_Role">User Role</option>' +
            '</select>' +
            '<select id="ddlOperation" class="search">' +
                '<option value="bw" selected="selected">begins with</option>' +
                '<option value="eq">equals</option>' +
                '<option value="cn">contains</option>' +
            '</select>' +
            '<input type="text" autocomplete="off" id="txtSearch" class="search" />' +
            '<input type="button" id="btnFind" value="Find" class="buttonsearch"/>' +
            '<input type="button" id="btnReset" value="Reset" class="buttonsearch" />' +
        '</div>');

            $("#btnFind").click(function() {
                searchurl = '?_search=true&searchField=' + $("#ddlField").val() + '&searchOper=' + $("#ddlOperation").val() + '&searchString=' + $("#txtSearch").val();
                GridBind(searchurl);
            });

            $("#btnReset").click(function() {
            searchurl = '';
            $("#ddlField").val("User_CAI");
            $("#ddlOperation").val("bw");
            $("#txtSearch").val("");
                GridBind(searchurl);
            });

        });

        function GridBind(searchurl) {
            $("#User_List").setGridParam({ page: 1, url: urlPath + '/User/GetAllUsers' + searchurl })
            $("#User_List").trigger("reloadGrid");

        } 
    </script>

</asp:Content>
