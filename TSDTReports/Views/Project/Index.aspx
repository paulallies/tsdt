<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<%@ Import Namespace="TSDTReports.Helpers" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Index
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h2>
        <%=Html.ViewData["Title"]%></h2>
    <form id="frmSearch" method="post" action='<%=ResolveUrl("~/Project/Index") %>'>
    <div class="pagerBox">
        <div style="float: left">
            <a href='<%=ResolveUrl("~/Project/Create")%>'>
                <img src='<%=ResolveUrl("~/images/new-16x16.png") %>' title="New" alt="New" /></a>
        </div>
    </div>

    <table id="Project_List" class="scroll" cellpadding="0" cellspacing="0">
    </table>
    <div id="pagerProject_List" class="scroll" style="text-align: center;">
    </div>
        <div id="ProjectOwners" style="overflow:scroll">
    </div>
    <div id="backgroundPopup">
    </div>
    </form>
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
    <script src="<%=HttpRuntime.AppDomainAppVirtualPath.TrimEnd('/') %>/Scripts/popup.js"  type="text/javascript"></script>

    <script src="<%=HttpRuntime.AppDomainAppVirtualPath.TrimEnd('/') %>/Scripts/jquery-jtemplates.js"
        type="text/javascript"></script>
    <script type="text/javascript">
    var urlPath = helper.rtrim('<%=ResolveUrl("~/")%>', "/");
        $(function() {
        //Click out event!
        $("#backgroundPopup").click(function() {
            resetPopup();
        });
                $("#Project_List").jqGrid({
                    url:  urlPath +  '/Project/GetAllProjects/',
                    datatype: 'json',
                    mtype: 'GET',
                    colNames: ['', 'ID', 'Project Number', 'SAP Number', 'WBS', 'Project Name', 'Status'],
                    colModel: [
          { name: 'act', index: 'ID', width: 75 },
          { name: 'Project_ID', index: 'ID', width: 50, align: 'left' },
          { name: 'Project_Number', index: 'Project_Number', width: 100, align: 'left' },
          { name: 'SAP_Number', index: 'SAP_Number', width: 150, align: 'left' },
          { name: 'WBS', index: 'WBS', width: 150, align: 'left' },
          { name: 'Project_Name', index: 'Project_Name', width: 400, align: 'left' },
          { name: 'Status', index: '', width: 100, align: 'left' }
          ],
                    pager: jQuery('#pagerProject_List'),
                    rowNum: 10,
                    rowList: [5, 10, 20, 50],
                    sortname: 'Id',
                    sortorder: "asc",
                    viewrecords: true,
                    imgpath: urlPath + '/scripts/themes/steel/images',
                    caption: ' ',
                    height: 220,
                    loadComplete: function() {
                        var ids = $("#Project_List").getDataIDs();
                        for (var i = 0; i < ids.length; i++) {
                            var cl = ids[i];
                            be = "<a href='#' onclick='OwnerPopUp(" + cl + ")'><img src='" + urlPath + "/images/user-group.png' title='Owners' alt='Owners' /></a><a href=" + urlPath + "/Project/Details/" + cl + " ><img src='" + urlPath + "/images/detail-16x16.png' title='Detail' alt='Detail' /></a>";
                            $("#Project_List").setRowData(ids[i], { act: be });
                        }
                    }
            });

            $("table[class=Header] th").append('<div style="float: left">' +
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
            searchurl = '';
            $("#ddlField").val("ID");
            $("#ddlOperation").val("bw");
            $("#txtSearch").val("");
                GridBind(searchurl);
            });

        });

        function GridBind(searchurl) {
            $("#Project_List").setGridParam({ page: 1, url: urlPath + '/Project/GetAllProjects' + searchurl })
            $("#Project_List").trigger("reloadGrid");

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
        function resetPopup() {
            disablePopup("ProjectOwners");
            $("#ProjectOwners").empty();
        } 
        
    </script>

</asp:Content>
