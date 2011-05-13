<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Index
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="headContent" runat="server">

    <script type="text/javascript">
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
                    else {

                        AddResourceToProject(ret.User_CAI, $("#ddlOwnerProjects").val());
                    }
                }
                else {
                    alert("Please select user");
                }

            });


        });

        function AddResourceToProject(User_CAI, Project_ID) {
            $.post(
            "/Resource/InsertProjectResource/",
            { Project_ID: Project_ID, User_CAI: User_CAI },
            function(data) {
                if (data.Message == false)
                    alert("Error inserting Project Resource");
                else
                    jQuery("#ProjectResources").trigger("reloadGrid");
            });
        }

        function DeleteProjectResource(Resource_ID) {
            if (confirm("Are you sure you want to delete")) {
                $.post(
                    "/Resource/DeleteProjectResource/",
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
            url: "/Resource/GetProjectResources/" + $("#ddlOwnerProjects").val(),
                datatype: 'json',
                mtype: 'GET',
                colNames: ['', 'CAI', 'First Name', 'Last Name'],
                colModel: [
                  { name: '', index: '', width: 50, align: 'left' },
                  { name: 'User_CAI', index: 'User_CAI', width: 50, align: 'left' },
                  { name: 'User_FirstName', index: 'User_FirstName', width: 100, align: 'left' },
                  { name: 'User_LastName', index: 'User_LastName', width: 150, align: 'left' }
                  ],
                pager: jQuery('#pagerProjectResources'),
                rowNum: 10,
                rowList: [5, 10, 20, 50],
                sortname: 'User_CAI',
                sortorder: "asc",
                viewrecords: true,
                imgpath: '/scripts/themes/steel/images',
                caption: 'Project Resources',
                height: 220
            });

            $("#UserList").jqGrid({
            url: "/User/GetUserList/",
                datatype: 'json',
                mtype: 'GET',
                colNames: ['CAI', 'First Name', 'Last Name'],
                colModel: [
                  { name: 'User_CAI', index: 'User_CAI', width: 50, align: 'left' },
                  { name: 'User_FirstName', index: 'User_FirstName', width: 100, align: 'left' },
                  { name: 'User_LastName', index: 'User_LastName', width: 150, align: 'left' }
                  ],
                pager: jQuery('#pagerUserList'),
                rowNum: 10,
                rowList: [10, 20, 50],
                sortname: 'User_CAI',
                sortorder: "asc",
                viewrecords: true,
                imgpath: '/scripts/themes/steel/images',
                caption: 'User List',
                height: 220
            });

        }

        function GetUserList() {
            $("#ProjectResources").setGridParam({ url: "/Resource/GetProjectResources/" + $("#ddlOwnerProjects").val() });
            $("#ProjectResources").trigger("reloadGrid");
        }

        function FillSelect(SelectControl) {
            $('#ddlOwnerProjects').find('option').remove().end().append('<option value="0">Select</option>').val('0');
            $.getJSON(
                    "/Resource/GetOwnerProjects",
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
    <h2>My Projects</h2>
    <p>
        &nbsp;</p>
    <div class="pagerBox">
        <div style="float: left">
            Select Owner:<%= Html.DropDownList("User_CAI", (SelectList)ViewData["Users"], "Select", new { style="font-size: 8pt"})%>
            &nbsp;&nbsp; Select Project:
            <select id="ddlOwnerProjects" style="font-size: 8pt">
                <option value="0">Select</option>
            </select>
        </div>
    </div>
    <table style="width: 800px">
        <tr>
            <td>
                <table id="UserList" class="scroll" cellpadding="0" cellspacing="0">
                </table>
                <div id="pagerUserList" class="scroll" style="text-align: center;">
                </div>
            </td>
            <td>
                <input type="button" value=">" id="btnAddToProject" style="margin-left: -30px" />
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
