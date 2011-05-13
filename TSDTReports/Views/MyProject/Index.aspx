<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Assign User(s) to Project
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="headContent" runat="server">

    <script type="text/javascript">

        //Global Variables
        var TSDT = {};
        TSDT.urlPath = helper.rtrim('<%=ResolveUrl("~/")%>', "/");
        TSDT.currentInput = 0;

        //Functions

        TSDT.FillSelect = function (SelectControl) {
            $('#ddlOwnerProjects').find('option').remove().end().append('<option value="0">Select</option>').val('0');
            $.ajax({
                type: "GET",
                url: '<%=ResolveUrl("~/MyProject/GetOwnerProjects")%>',
                data: { User_CAI: $("#" + SelectControl).val() },
                dataType: "json",
                success: function(Json) {
                    var drp = document.getElementById("ddlOwnerProjects");
                    $.each(Json.data, function(i, val) {
                        drp.options[drp.options.length] = new Option(val.details, val.id);
                    });
                }
            });
        }

        TSDT.InitializeGrids = function () {
            $("#ProjectResources").jqGrid({
                url: TSDT.urlPath + '/MyProject/GetProjectResources/' + $("#ddlOwnerProjects").val(),
                datatype: 'json',
                mtype: 'GET',
                colNames: ['id', 'CAI', 'First Name', 'Last Name', 'Enabled', 'Project Hours'],
                colModel: [
                  { name: 'id', index: 'id', width: 50, align: 'left' },
                  { name: 'User_CAI', index: 'User_CAI', width: 50, align: 'left' },
                  { name: 'User_FirstName', index: 'User_FirstName', width: 100, align: 'left' },
                  { name: 'User_LastName', index: 'User_LastName', width: 150, align: 'left' },
                  { name: 'Active', index: 'Active', width: 55, align: 'center', formatter: 'checkbox' },
                  { name: 'PO', index: 'ProjectHours', width: 80, align: 'right' }

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
                height: 220,
                loadComplete: function() {
                    var ids = $("#ProjectResources").getDataIDs();
                    var objRowData = new Array();
                    if (ids != null) {
                        for (var i = 0; i < ids.length; i++) {
                            var cl = ids[i];
                            objRowData = $("#ProjectResources").getRowData(cl);
                            if ($.trim(objRowData["PO"]).length > 0)
                                be = "<span style='color:blue' onclick='TSDT.ChangeProjectHours(" + cl + ");'  id='PH" + cl + "' >" + objRowData["PO"] + "</span>";
                            else
                                be = "<span style='color:blue' onclick='TSDT.ChangeProjectHours(" + cl + ");' id='PH" + cl + "' >" + "0.00" + "</span>";
                            $("#ProjectResources").setRowData(ids[i], { PO: be });
                        }
                    }
                }
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
                TSDT.GridBind(searchurl);
            });

            $("#btnDisableAllUsersFromProject").click(function() {
                if ($("#ddlOwnerProjects").val() > 0) {
                    TSDT.ChangeActiveStatusForAll(false);
                }
                else {
                    alert("Please select project");
                }
            });

            $("#btnEnableAllUsersFromProject").click(function() {
                if ($("#ddlOwnerProjects").val() > 0) {
                    TSDT.ChangeActiveStatusForAll(true);
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
                        TSDT.ChangeActiveStatusForUser(ret.id, false);
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
                        TSDT.ChangeActiveStatusForUser(ret.id, true);
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
                TSDT.GridBind(searchurl);
            });

        }

        TSDT.AddResourceToProject = function(User_CAI, Project_ID) {
            $.ajax({
                type: "POST",
                url: '<%=ResolveUrl("~/MyProject/InsertProjectResource")%>',
                data: { Project_ID: Project_ID, User_CAI: User_CAI },
                dataType: "json",
                success: function(data) {
                    if (data.Status == false)
                        alert(data.Message);
                    else {
                        $("#ProjectResources").trigger("reloadGrid");
                    }
                }
            });
        }

        TSDT.DeleteProjectResource = function(Resource_ID) {
            if (confirm("Are you sure you want to remove user from project")) {
                $.ajax({
                    type: "POST",
                    url: '<%=ResolveUrl("~/MyProject/DeleteProjectResource")%>',
                    data: { Resource_ID: Resource_ID },
                    dataType: "json",
                    success: function(data) {
                        if (data.Status == false)
                            alert(data.Message);
                        else {
                            $("#ProjectResources").trigger("reloadGrid");
                        }
                    }
                });
            }
        }



        TSDT.ChangeActiveStatusForUser = function(resource_ID, Activestatus) {
            $.ajax({
                type: "POST",
                url: '<%=ResolveUrl("~/MyProject/ChangeActiveStatusForUser")%>',
                data: { Resource_ID: resource_ID, status: Activestatus },
                dataType: "json",
                success: function(data) {
                    if (data.Status == false)
                        alert(data.Message);
                    else {
                        $("#ProjectResources").trigger("reloadGrid");
                    }
                }
            });
        }

        TSDT.ChangeActiveStatusForAll = function(Activestatus) {
            var project_ID = $("#ddlOwnerProjects").val();
            $.ajax({
                type: "POST",
                url: '<%=ResolveUrl("~/MyProject/ChangeActiveStatusForAll")%>',
                data: { Project_ID: project_ID, status: Activestatus },
                dataType: "json",
                success: function(data) {
                    if (data.Status == false)
                        alert(data.Message);
                    else {
                        $("#ProjectResources").trigger("reloadGrid");
                    }
                }
            });
        }

        TSDT.GridBind = function(searchurl) {
            $("#UserList").setGridParam({ page: 1, url: TSDT.urlPath + '/User/GetUsers' + searchurl })
            $("#UserList").trigger("reloadGrid");

        }

        TSDT.GetUserList = function() {
            $("#ProjectResources").setGridParam({ url: TSDT.urlPath + '/MyProject/GetProjectResources/' + $("#ddlOwnerProjects").val() });
            $("#ProjectResources").trigger("reloadGrid");
        }

        TSDT.ChangeProjectHours = function(clientid) {
            var tempval = $("#PH" + clientid).text();

            $("#PH" + clientid).replaceWith("<input style='font-size:8pt; width:65px; text-align:right' maxlength='7' type='text' id='InputPH" + clientid + "' value=" + tempval + " />");
            $("#InputPH" + clientid).focus().keydown(function(event) {
                if (event.keyCode == 27) {
                    $("#InputPH" + clientid).replaceWith("<span style='color:blue' onclick='TSDT.ChangeProjectHours(" + clientid + ");' id='PH" + clientid + "' >" + tempval + "</span>");
                }
                if (
               (event.keyCode > 47 && event.keyCode < 59) ||
               event.keyCode == 190 ||
               event.keyCode == 8 ||
               event.keyCode == 37 ||
               event.keyCode == 39 ||
               event.keyCode == 46 ||
               event.keyCode == 110 ||
               (event.keyCode > 95 && event.keyCode < 106)

               )
                    return true;
                else
                    return false;
            });
            $("#InputPH" + clientid).keydown(function(event) {
                if (event.keyCode == 13) {
                    $.ajax({
                        type: "POST",
                        url: '<%=ResolveUrl("~/MyProject/ChangeProjectHours")%>',
                        data: { ResourceID: clientid, ProjectHours: $(this).val() },
                        dataType: "json",
                        success: function(data) {
                            if (data.Message === "Success") {
                                var hourval = parseFloat($("#InputPH" + clientid).val());
                                $("#InputPH" + clientid).replaceWith("<span style='color:blue' onclick='TSDT.ChangeProjectHours(" + clientid + ");' id='PH" + clientid + "' >" + hourval.toFixed(2) + "</span>");
                            }
                        }
                    });
                }
            });

            $("#InputPH" + clientid).blur(function() {
                $("#InputPH" + clientid).replaceWith("<span style='color:blue' onclick='TSDT.ChangeProjectHours(" + clientid + ");' id='PH" + clientid + "' >" + tempval + "</span>");

            });
        }

        $(function() {

            TSDT.FillSelect("User_CAI");
            TSDT.InitializeGrids();

            $('#User_CAI').change(function() {
                TSDT.FillSelect("User_CAI");
                TSDT.GetUserList();
            });

            $('#ddlOwnerProjects').change(function() {
                TSDT.GetUserList();

            });

            $('#btnAddToProject').click(function() {
                var id = $("#UserList").getGridParam('selrow');
                if (id) {
                    var ret = $("#UserList").getRowData(id);
                    if ($("#ddlOwnerProjects").val() == 0) {
                        alert("Please Select Project");
                    }
                    else
                        TSDT.AddResourceToProject(ret.User_CAI, $("#ddlOwnerProjects").val());
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
                        TSDT.DeleteProjectResource(ret.id);
                }
                else {
                    alert("Please select user to remove");
                }
            });


        });



        




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
