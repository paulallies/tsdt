<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Assign User(s) to Project
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="headContent" runat="server">

    <script type="text/javascript">
        var urlPath = rtrim('<%=ResolveUrl("~/")%>', "/");
        $(function() {
            FillSelect("User_CAI");
            InitializeGrids();

            $('#User_CAI').change(function() {
                FillSelect("User_CAI");
                GetUserList();
            });

            $('#ddlOwnerProjects').change(function() {
                GetUserList();

            });

            $('#btnAddToProject').click(function() {
                var id = $("#UserList").getGridParam('selrow');
                if (id) {
                    var ret = $("#UserList").getRowData(id);
                    if ($("#ddlOwnerProjects").val() == 0) {
                        alert("Please Select Project");
                    }
                    else
                        AddResourceToProject(ret.User_CAI, $("#ddlOwnerProjects").val());
                }
                else {
                    alert("Please select user");
                }
            });

            $('#btnRemoveFromProject').click(function() {
                var id = $("#ProjectResources").getGridParam('selrow');
                if (id) {
                    var ret = $("#ProjectResources").getRowData(id);
                    if ($("#ddlOwnerProjects").val() == 0) {
                        alert("Please Select Project");
                    }
                    else
                        DeleteProjectResource(ret.id);
                }
                else {
                    alert("Please select user to remove");
                }
            });


        });

        function AddResourceToProject(User_CAI, Project_ID) {
            $.post(
            '<%=ResolveUrl("~/MyProject/InsertProjectResource/")%>',
            { Project_ID: Project_ID, User_CAI: User_CAI },
            function(data) {
                if (data.Message == false)
                    alert("Error inserting Project Resource");
                else
                    jQuery("#ProjectResources").trigger("reloadGrid");
            });
        }

        function DeleteProjectResource(Resource_ID) {
            if (confirm("Are you sure you want to remove user from project")) {
                $.post(
                    '<%=ResolveUrl("~/MyProject/DeleteProjectResource/")%>',
                    { Resource_ID: Resource_ID },
                    function(data) {
                        if (data.Message == false)
                            alert("Error Deleting Resource");
                        else
                            jQuery("#ProjectResources").trigger("reloadGrid");
                    });
            }
        }

        function InitializeGrids() {

            $("#ProjectResources").jqGrid({
                url: urlPath + '/MyProject/GetProjectResources/' + $("#ddlOwnerProjects").val(),
                datatype: 'json',
                mtype: 'GET',
                colNames: ['id', 'CAI', 'First Name', 'Last Name', 'Enabled'],
                colModel: [
                  { name: 'id', index: 'id', width: 50, align: 'left' },
                  { name: 'User_CAI', index: 'User_CAI', width: 50, align: 'left' },
                  { name: 'User_FirstName', index: 'User_FirstName', width: 100, align: 'left' },
                  { name: 'User_LastName', index: 'User_LastName', width: 150, align: 'left' },
                  { name: 'Active', index: 'Active', width: 55, align: 'center', formatter: 'checkbox' }

                  ],
                pager: jQuery('#pagerProjectResources'),
                rowNum: 10,
                rowList: [5, 10, 20, 50],
                sortname: 'User_CAI',
                sortorder: "asc",
                viewrecords: true,
                imgpath: '<%=ResolveUrl("~/scripts/themes/steel/images")%>',
                caption: ' ',
                // multiselect : true,
                height: 220
            });

            $("#UserList").jqGrid({
                url: '<%=ResolveUrl("~/User/GetUsers/")%>',
                datatype: 'json',
                mtype: 'GET',
                colNames: ['CAI', 'First Name', 'Last Name'],
                colModel: [
                  { name: 'User_CAI', index: 'User_CAI', width: 80, align: 'left' },
                  { name: 'User_FirstName', index: 'User_FirstName', width: 150, align: 'left' },
                  { name: 'User_LastName', index: 'User_LastName', width: 150, align: 'left' }
                  ],
                pager: jQuery('#pagerUserList'),
                rowNum: 10,
                rowList: [10, 20, 50],
                sortname: 'User_CAI',
                sortorder: "asc",
                viewrecords: true,
                imgpath: '<%=ResolveUrl("~/scripts/themes/steel/images")%>',
                caption: ' ',
                height: 220
            });



            $("table[class=Header] th").eq(0).append('<div style="float: left">' +
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

            $("table[class=Header] th").eq(1).append('<table style="width:100%">' +
            '<tr>' +
            '<td>Project Resources</td>' +
            '<td>' +
            '<input type="button" id="btnDisableAllUsersFromProject" class="search" value="Disable all" />' +
            '<input type="button" id="btnEnableAllUsersFromProject" class="search" value="Enable all" />' +
            '<input type="button" id="btnDisableSelected" class="search" value="Disable" />' +
            '<input type="button" id="btnEnableSelected" class="search" value="Enable" />' +
            '</td></tr></table>');

            $("#btnFind").click(function() {
                searchurl = '?_search=true&searchField=' + $("#ddlField").val() + '&searchOper=' + $("#ddlOperation").val() + '&searchString=' + $("#txtSearch").val();
                GridBind(searchurl);
            });

            $("#btnDisableAllUsersFromProject").click(function() {
                if ($("#ddlOwnerProjects").val() > 0) {
                    ChangeActiveStatusForAll(false);
                }
                else {
                    alert("Please select project");
                }
            });

            $("#btnEnableAllUsersFromProject").click(function() {
                if ($("#ddlOwnerProjects").val() > 0) {
                    ChangeActiveStatusForAll(true);
                }
                else {
                    alert("Please select project");
                }
            });

            $("#btnDisableSelected").click(function() {
                var id = $("#ProjectResources").getGridParam('selrow');
                if (id) {
                    var ret = $("#ProjectResources").getRowData(id);
                    if ($("#ddlOwnerProjects").val() == 0) {
                        alert("Please Select Project");
                    }
                    else
                        ChangeActiveStatusForUser(ret.id, false);
                }
                else {
                    alert("Please select user");
                }
            });

            $("#btnEnableSelected").click(function() {
                var id = $("#ProjectResources").getGridParam('selrow');
                if (id) {
                    var ret = $("#ProjectResources").getRowData(id);
                    if ($("#ddlOwnerProjects").val() == 0) {
                        alert("Please Select Project");
                    }
                    else
                        ChangeActiveStatusForUser(ret.id, true);
                }
                else {
                    alert("Please select user");
                }
            });

            $("#btnReset").click(function() {
                searchurl = '';
                $("#ddlField").val("User_CAI");
                $("#ddlOperation").val("bw");
                $("#txtSearch").val("");
                GridBind(searchurl);
            });

        }

        function ChangeActiveStatusForUser(resource_ID, Activestatus) {
            $.post(
            '<%=ResolveUrl("~/MyProject/ChangeActiveStatusForUser/")%>',
            { Resource_ID: resource_ID, status: Activestatus },
            function(data) {
                if (data.Message == false)
                    alert("Error changing status");
                else
                    jQuery("#ProjectResources").trigger("reloadGrid");
            });

        }

        function ChangeActiveStatusForAll(Activestatus) {
            var project_ID = $("#ddlOwnerProjects").val();
            $.post(
                    '<%=ResolveUrl("~/MyProject/ChangeActiveStatusForAll/")%>',
                    { Project_ID: project_ID, status: Activestatus },
                    function(data) {
                        if (data.Message == false)
                            alert("Error Changing Status");
                        else
                            jQuery("#ProjectResources").trigger("reloadGrid");
                    });
        }

        function GridBind(searchurl) {
            $("#UserList").setGridParam({ page: 1, url: urlPath +  '/User/GetUsers' + searchurl })
            $("#UserList").trigger("reloadGrid");

        }

        function GetUserList() {
            $("#ProjectResources").setGridParam({ url: urlPath + '/MyProject/GetProjectResources/' + $("#ddlOwnerProjects").val() });
            $("#ProjectResources").trigger("reloadGrid");

        }

        function FillSelect(SelectControl) {
            $('#ddlOwnerProjects').find('option').remove().end().append('<option value="0">Select</option>').val('0');
            $.getJSON(
                     '<%=ResolveUrl("~/MyProject/GetOwnerProjects")%>',
                    { "User_CAI": $("#" + SelectControl).val() },
                    function(Json) {
                        var drp = document.getElementById("ddlOwnerProjects");
                        $.each(Json.data, function(i, val) {
                            drp.options[drp.options.length] = new Option(val.details, val.id);
                        });

                    }
                );
        }


    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h2>Assign User(s) to Project</h2>
    <p>
        &nbsp;</p>
    <div class="pagerBox">
        <div style="float: left">
            <%if (ViewData["Role"].ToString() == "ProjectOwner")
              {%>
            Select a Owner:<%=Html.DropDownList("User_CAI", (SelectList)ViewData["Users"], "Select", new { style = "font-size: 8pt",  disabled="disabled"})%>
            <%}
              else
              { %>
            Select a Owner:<%=Html.DropDownList("User_CAI", (SelectList)ViewData["Users"], "Select", new { style = "font-size: 8pt" })%>
            <%} %>
            &nbsp;&nbsp; Select a Project:
            <select id="ddlOwnerProjects" style="font-size: 8pt">
                <option value="0">Select</option>
            </select>
        </div>
    </div>
    <table style="width: 900px">
        <tr>
            <td>
                <table id="UserList" class="scroll" cellpadding="0" cellspacing="0">
                </table>
                <div id="pagerUserList" class="scroll" style="text-align: center;">
                </div>
            </td>
            <td>
                <input type="button" value=">" id="btnAddToProject" />
                <br />
                <input type="button" value="<" id="btnRemoveFromProject" />
            </td>
            <td>
                <table id="ProjectResources" class="scroll" cellpadding="0" cellspacing="0">
                </table>
                <div id="pagerProjectResources" class="scroll" style="text-align: center;">
                </div>
            </td>
        </tr>
    </table>
</asp:Content>
