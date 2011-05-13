<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	List
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">


<h2>My Grid Data</h2>
<table id="list" class="scroll" cellpadding="0" cellspacing="0"></table>
<div id="pager" class="scroll" style="text-align:center;"></div>



</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="headContent" runat="server">

<script type="text/javascript">
    $(document).ready(function() {
        $("#list").jqGrid({
            url: '/Project/GridData/',
            datatype: 'json',
            mtype: 'GET',
            colNames: ['ID', 'Project Number', 'SAP Number', 'WBS', 'Project Name', 'Status'],
            colModel: [
          { name: 'Project_ID', index: 'ID', width: 50, align: 'left' },
          { name: 'Project_Number', index: 'Project_Number', width: 100, align: 'left' },
          { name: 'SAP_Number', index: 'SAP_Number', width: 150, align: 'left' },
          { name: 'WBS', index: 'WBS', width: 150, align: 'left' },
          { name: 'Project_Name', index: 'Project_Name', width: 400, align: 'left', search: true },
          { name: 'Status', index: '', width: 100, align: 'left' }
          ],
            pager: jQuery('#pager'),
            rowNum: 10,
            rowList: [5, 10, 20, 50],
            sortname: 'Id',
            sortorder: "asc",
            viewrecords: true,
            imgpath: '/scripts/themes/steel/images',
            caption: 'Projects',
            sopt: ['gt', 'ew']
        }).navGrid('#pager', { edit: false, add: false, del: false, search: true });
        $.jgrid.search = {
            caption: "Search...",
            Find: "Find",
            Reset: "Reset",
            odata: ['equal', 'not equal', 'less', 'less or equal', 'greater', 'greater or equal', 'begins with', 'ends with', 'contains'],
            sopt: ['bw','eq','cn']
        };
    }); 
</script>
</asp:Content>
