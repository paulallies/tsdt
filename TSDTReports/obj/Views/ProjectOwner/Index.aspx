<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<%@ Import Namespace="TSDTReports.Helpers" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Project Owners
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="headContent" runat="server">
    <style type="text/css">
        #backgroundPopup
        {
            display: none;
            position: fixed;
            _position: absolute; /* hack for internet explorer 6*/
            height: 100%;
            width: 100%;
            top: 0;
            left: 0;
            background: #000000;
            border: 1px solid #cecece;
            z-index: 999;
        }
        #ProjectOwners
        {
            display: none;
            position: fixed;
            _position: absolute; /* hack for internet explorer 6*/
            height: 250px;
            width: 250px;
            background: #FFFFFF;
            border: 2px solid #cecece;
            z-index: 1000;
            padding: 12px;
            font-size: 13px;
        }
    </style>

    <script type="text/html" id="TemplateResultsTable">  
       {#template MAIN}  
       <h2>Project Owners</h2>
       <table id="mytable" class="Control" style="border-collapse: collapse;" border="1" rules="all" cellspacing="0" cellpadding="3">  
         <tr style="background-color: #009dd9; color: white; ">  
            <th>CAI</th>  
           <th>Surname</th>  
           <th>First Name</th>
         </tr>  
         <tbody>
           {#foreach $T.ProjectOwners as u}
        <tr>
        <td>{$T.u.User_CAI}</td>
        <td>{$T.u.User_LastName}</td>
        <td>{$T.u.User_FirstName}</td>
        </tr>
            {#/for}
            </tbody>
         </table>
         <br/>      
         <input type="button" value="Close" id="btnClose" onclick="resetPopup()"/>
      {#/template MAIN}

    </script>


    <script src="<%=HttpRuntime.AppDomainAppVirtualPath.TrimEnd('/') %>/Scripts/popup.js"
        type="text/javascript"></script>

    <script src="<%=HttpRuntime.AppDomainAppVirtualPath.TrimEnd('/') %>/Scripts/jquery-jtemplates.js"
        type="text/javascript"></script>

    <script type="text/javascript">
        var urlPath = rtrim('<%=ResolveUrl("~/")%>', "/");

        function projectsOwned() {
            jQuery("#ProjectOwned").setGridParam({ url: urlPath + '/ProjectOwner/GridData/' + $("#User_CAI").val() })
            jQuery("#ProjectOwned").trigger("reloadGrid");
        }


        function resetPopup() {
            disablePopup("ProjectOwners");
            $("#ProjectOwners").empty();
        }
        
        $(document).ready(function() {
        //Click out event!
        $("#backgroundPopup").click(function() {
        resetPopup();
        });
        
            $("#ProjectOwners").draggable();
            $("#ProjectOwned").jqGrid({
                url: urlPath + '/ProjectOwner/GridData/' + $("#User_CAI").val(),
                datatype: 'json',
                mtype: 'GET',
                colNames: ['', 'ID', 'Project Number', 'SAP Number', 'WBS', 'Project Name', 'Status'],
                colModel: [
                  { name: 'act', index: 'ID', width: 50, align: 'left' },
                  { name: 'Project_ID', index: 'ID', width: 50, align: 'left' },
                  { name: 'Project_Number', index: 'Project_Number', width: 100, align: 'left' },
                  { name: 'SAP_Number', index: 'SAP_Number', width: 150, align: 'left' },
                  { name: 'WBS', index: 'WBS', width: 150, align: 'left' },
                  { name: 'Project_Name', index: 'Project_Name', width: 400, align: 'left' },
                  { name: 'Status', index: '', width: 100, align: 'left' }
                  ],
                pager: jQuery('#pagerProjectsOwned'),
                rowNum: 10,
                rowList: [5, 10, 20, 50],
                sortname: 'Id',
                sortorder: "asc",
                viewrecords: true,
                imgpath: urlPath + '/scripts/themes/steel/images',
                caption: 'Projects Owned',
                loadComplete: function() {
                    var ids = $("#ProjectOwned").getDataIDs();
                    for (var i = 0; i < ids.length; i++) {
                        var cl = ids[i];
                        be = "<a href='#' onclick='OwnerPopUp(" + cl + ")'><img src='" + urlPath + "/images/user-group.png' title='Owners' alt='Owners' /></a><a href='#' onclick='DeletePO(" + cl + ")'><img src='" + urlPath + "/images/delete.png' title='Delete' alt='Delete' /></a>";
                        $("#ProjectOwned").setRowData(ids[i], { act: be });
                    }
                }
            });

            $("#ProjectList").jqGrid({
                url: urlPath + '/Project/GetAllProjects/',
                datatype: 'json',
                mtype: 'GET',
                colNames: ['', 'ID', 'Project Number', 'SAP Number', 'WBS', 'Project Name', 'Status'],
                colModel: [
          { name: 'act', index: 'ID', width: 50, align: 'left' },
          { name: 'Project_ID', index: 'ID', width: 50, align: 'left' },
          { name: 'Project_Number', index: 'Project_Number', width: 100, align: 'left' },
          { name: 'SAP_Number', index: 'SAP_Number', width: 150, align: 'left' },
          { name: 'WBS', index: 'WBS', width: 150, align: 'left' },
          { name: 'Project_Name', index: 'Project_Name', width: 400, align: 'left' },
          { name: 'Status', index: '', width: 100, align: 'left' }
          ],
                pager: jQuery('#pagerProjects'),
                rowNum: 10,
                rowList: [5, 10, 20, 50],
                sortname: 'Id',
                sortorder: "asc",
                viewrecords: true,
                imgpath: urlPath + '/scripts/themes/steel/images',
                caption: ' ',
                height: 220,
                loadComplete: function() {
                    var ids = $("#ProjectList").getDataIDs();
                    for (var i = 0; i < ids.length; i++) {
                        var cl = ids[i];
                        be = "<a href='#' onclick='OwnerPopUp(" + cl + ")'><img src='" + urlPath + "/images/user-group.png' title='Owners' alt='Owners' /></a><a href='#' onclick='InsertProject(" + cl + ")'><img src='" + urlPath + "/images/add.png' title='Add' alt='Add' /></a>";
                        $("#ProjectList").setRowData(ids[i], { act: be });
                    }
                }
            });

            $("table[class=Header] th").eq(1).append('<div style="float: left">' +
        'Filter:' +
            '<select id="ddlField" class="search">' +
             '   <option value="ID" selected="selected">ID</option>' +
              '  <option value="Project_Number">Project Number</option>' +
               ' <option value="SAP_Number">SAP</option>' +
                '<option value="WBS">WBS</option>' +
                '<option value="Project_Name">Project Name</option>' +
                '<option value="Status">Status</option>' +
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
                $("#ddlField").val("ID");
                $("#txtSearch").val("");
                $("#ddlOperation").val("bw");
                searchurl = '';
                GridBind(searchurl);
            });


            $("#User_CAI").change(function() {
                projectsOwned();
            });

        });

        function InsertProject(Project_ID) {
            $.post(urlPath + '/ProjectOwner/InsertProject/',
            { Project_ID: Project_ID, User_CAI: $("#User_CAI").val() },
            function(data) {
                if (data.Message == false)
                    alert("Error inserting Project");
                else
                    jQuery("#ProjectOwned").trigger("reloadGrid");
            });
        }

        function GridBind(searchurl) {
            $("#ProjectList").setGridParam({ page: 1, url: urlPath + '/Project/GetAllProjects' + searchurl })
            $("#ProjectList").trigger("reloadGrid");

        }

        function DeleteProjectOwnerProject(Project_ID, User_CAI) {
            $.post(
            '<%=ResolveUrl("~/ProjectOwner/DeleteProject/")%>',
            { Project_ID: Project_ID, User_CAI: User_CAI },
            function(data) {
                if (data.Message == false)
                    alert("Error Deleting Project");
                else
                    jQuery("#ProjectOwned").trigger("reloadGrid");
            });
        }

        function DeletePO(id) {
            if (confirm("Are you sure you want to delete"))
                DeleteProjectOwnerProject(id, $("#User_CAI").val());
            else
                alert("Didn't delete");
        }

        function OwnerPopUp(id) {
            var surname = $("#User_LastName").val();
            var firstname = $("#User_FirstName").val();
            var CAI = $("#CAI").text();
            $.ajax({
                type: "GET",
                url: '<%=ResolveUrl("~/ProjectOwner/GetProjectOwners?projectID=' + id + '")%>',
                dataType: "json",
                success: function(data) {
                    if (data.Message == false)
                        alert("Error");
                    else {
                        if (data.ProjectOwners.length > 0) {
                            ApplyTemplate(data);
                            loadPopup("ProjectOwners");
                            centerPopup("ProjectOwners");
                        }
                        else
                            alert("No Project Owners!");

                    }
                }
            });

        }

        function ApplyTemplate(data) {
            $("#ProjectOwners").setTemplate($("#TemplateResultsTable").html());
            $("#ProjectOwners").processTemplate(data);
        }

            
        
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="pagerBox">
        <div style="float: left">
            Select a Owner:<%= Html.DropDownList("User_CAI", (SelectList)ViewData["Users"], "Select", new { style="font-size : 8pt"})%>
        </div>
    </div>
    <table id="ProjectOwned" class="scroll" cellpadding="0" cellspacing="0">
    </table>
    <div id="pagerProjectsOwned" class="scroll" style="text-align: center;">
    </div>
    <br />
    <table id="ProjectList" class="scroll" cellpadding="0" cellspacing="0">
    </table>
    <div id="pagerProjects" class="scroll" style="text-align: center;">
    </div>
    <div id="ProjectOwners" style="overflow:scroll">
    </div>
    <div id="backgroundPopup">
    </div>
</asp:Content>
