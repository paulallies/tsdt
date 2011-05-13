<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<TSDTReports.Models.tbl_User>" %>

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
                        <td class="inputlabel">User CAI:</td>
                        <td class="inputdata">
                            <span id="CAI">
                                <%=Model.User_CAI %></span></td>
                    </tr>
                    <tr>
                        <td class="inputlabel">User First Name:</td>
                        <td class="inputdata">
                            <%= Html.TextBox("User_FirstName", Model.User_FirstName) %>
                            <%= Html.ValidationMessage("User_FirstName", "*") %></td>
                    </tr>
                    <tr>
                        <td class="inputlabel">User Last Name:</td>
                        <td class="inputdata">
                            <%= Html.TextBox("User_LastName", Model.User_LastName) %>
                            <%= Html.ValidationMessage("User_LastName", "*") %></td>
                    </tr>
                    <tr>
                        <td class="inputlabel">Employee Number:</td>
                        <td class="inputdata"><%= Html.TextBox("EmployeeNumber", Model.EmployeeNumber)%></td>
                    </tr>
                    <tr>
                        <td class="inputlabel">Company Name:</td>
                        <td class="inputdata">
                            <%= Html.TextBox("CompanyName", Model.CompanyName) %></td>
                    </tr>
                    <tr>
                        <td class="inputlabel">Company ID:</td>
                        <td class="inputdata">
                            <%= Html.TextBox("CompanyID", Model.CompanyID) %></td>
                    </tr>
                    <tr>
                        <td class="inputlabel">User Role:</td>
                        <td class="inputdata">
                            <%= Html.DropDownList("User_Role", (SelectList)ViewData["Roles"])%></td>
                    </tr>
                    <tr>
                        <td class="inputlabel">Active:</td>
                        <td class="inputdata">
                            <%= Html.CheckBox("Active", (bool)ViewData["Active"])%></td>
                    </tr>
                   <tr>
                        <td class="inputlabel">Photo:</td>
                        <td class="inputdata"><img id="imgPhoto" title="" alt="" /></td>
                    </tr>
                    <tr>
                        <td class="inputlabel"><input id="btnDisplayLookUp" value="Saco Lookup" type="button" /></td>
                        <td class="inputdata"><a href="javascript:$('form').submit();">
                            <img src="<%=HttpRuntime.AppDomainAppVirtualPath.TrimEnd('/') %>/images/check-16x16.png" /></a> <a href="<%=HttpRuntime.AppDomainAppVirtualPath.TrimEnd('/') %>/User/Details/<%=Model.User_CAI %>">
                            <img src="<%=HttpRuntime.AppDomainAppVirtualPath.TrimEnd('/') %>/images/undo-16x16.png" title="Cancel" alt="cancel" /></a> </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    
    <% } %>
    <div id="SacoLookup" style="overflow: scroll;">
    </div>
    <div id="backgroundPopup">
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="headContent" runat="server">

    <script src="<%=HttpRuntime.AppDomainAppVirtualPath.TrimEnd('/') %>/Scripts/popup.js" type="text/javascript"></script>

    <script src="<%=HttpRuntime.AppDomainAppVirtualPath.TrimEnd('/') %>/Scripts/jquery-jtemplates.js" type="text/javascript"></script>

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
        #SacoLookup
        {
            display: none;
            position: fixed;
            _position: absolute; /* hack for internet explorer 6*/
            height: 250px;
            width: 500px;
            background: #FFFFFF;
            border: 2px solid #cecece;
            z-index: 1000;
            padding: 12px;
            font-size: 13px;
        }
        #mytable
        {
            border-collapse: collapse;
            width: 300px;
        }
        #mytable th, #mytable td
        {
            border: 1px solid #000;
            padding: 3px;
        }
        #mytable tr.highlight
        {
            background-color: blue;
            color: White;
            cursor: pointer, hand;
        }
    </style>

    <script type="text/html" id="TemplateResultsTable">  
       {#template MAIN}  
       <h2>Saco Lookup</h2>
       <table id="mytable" class="Control" style="border-collapse: collapse;" border="1" rules="all" cellspacing="0" cellpadding="3">  
         <tr style="background-color: #009dd9; color: white; ">  
            <th>Surname</th>  
           <th>Firstname</th>  
           <th>Company</th>
           <th>Company ID</th>               
           <th>Employee Number</th>  
         </tr>  
         <tbody>
           {#foreach $T.employees as e}
        <tr>
        <td>{$T.e.employee_surname}</td>
        <td>{$T.e.employee_firstname}</td>
        <td>{$T.e.employee_company}</td>
        <td>{$T.e.employee_companyid}</td>      
        <td>{$T.e.employee_number}</td>
        </tr>
            {#/for}
            </tbody>
         </table>
         <br/>
         <input type="button" onclick="resetSacoLookup();" value="Cancel" />
      {#/template MAIN}



  
    </script>

    <script type="text/javascript">


        $(function() {

            if($.trim($("#EmployeeNumber").val()).length > 0)
            $("#imgPhoto").attr("src", "http://zarfweb.chevron.com/SACO2/photo.ashx?id=" + $("#EmployeeNumber").val());

            $("#btnDisplayLookUp").click(function() {
                var surname = $("#User_LastName").val();
                var firstname = $("#User_FirstName").val();
                var CAI = $("#CAI").text();
                $.ajax({
                    type: "GET",
                    url: '<%=ResolveUrl("~/User/GetUserEmployees?cai=' + CAI + '")%>',
                    dataType: "json",
                    success: function(data) {
                        if (data.Status == false)
                            alert(data.Message);
                        else {
                            ApplyTemplate(data);
                            loadPopup("SacoLookup");
                            centerPopup("SacoLookup");
                        }
                    }
                });

            });

            //Click out event!
            $("#backgroundPopup").click(function() {
                resetSacoLookup();
            });
    




        });

        function resetSacoLookup() {
            disablePopup("SacoLookup");
            $("#SacoLookup").empty();
        }

        function error(error) {
            alert("error: " + error);
        }

        function callback() {
            alert("done:" + responseText);
        }


        function ApplyTemplate(data) {
            $("#SacoLookup").setTemplate($("#TemplateResultsTable").html());
            $("#SacoLookup").processTemplate(data);

            $("#mytable tr").hover(function() {
                $(this).addClass("highlight");
            },
                function() {
                    $(this).removeClass("highlight");
                }
            );

            $("#mytable tr").click(function() {
                var empNumber = ($(this).find("td:eq(4)").text());
                var compID = ($(this).find("td:eq(3)").text());
                var compName = ($(this).find("td:eq(2)").text());
                $("#EmployeeNumber").val(empNumber);
                $("#CompanyID").val(compID);
                $("#CompanyName").val(compName);
                $("#imgPhoto").attr("src", "http://zarfweb.chevron.com/SACO2/photo.ashx?id=" + $("#EmployeeNumber").val());

                resetSacoLookup();
            });

        }
   
   
    </script>

</asp:Content>
