<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<TSDTReports.Models.TimeSheetPackage>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Index
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <% using (Html.BeginForm("index", "Timesheet", FormMethod.Post))
       { %>
    <table>
        <tr>
            <td>User: </td>
            <td>
                <%if ((string)ViewData["Role"] == "Administrator")
                  {%>
                <%=Html.DropDownList("ddlUsers", (SelectList)ViewData["Users"], "Select", new { style = "font-size: 8pt"})%>
                <%}
                  else
                  { %>
                <%=Html.DropDownList("ddlUsers", (SelectList)ViewData["Users"], "Select", new { style = "font-size: 8pt",  disabled="disabled"})%>
                <%} %>
            </td>
            <td>Date: </td>
            <td>
                <%=Html.TextBox("txtDate", Model.Date.ToString("dd-MMM-yyyy"), new { style = "font-size: 8pt"})%>
            </td>

        </tr>
    </table>
    <br />
    <input type="button" id="btnViewProjects" value="Hide/Show" />
    <%if (Model.TimeSheets.Count > 0)
      { %>
    <div style="overflow: scroll; padding: 5px;">
        <table class="Control" id="tblTimeSheets" style="background-color: white; width: 2000px;
            border-collapse: collapse;" border="1" rules="all" cellspacing="0" cellpadding="3">
            <!-- Header -->
            <tr>
                <th style="width: 50px"></th>
                <th style="width: 50px">Status </th>
                <th style="width: 50px">Project&nbsp;No.</th>
                <th style="width: 100px">SAP </th>
                <th style="width: 130px">Cost Code </th>
                <th style="width: 300px">Project Name </th>
                <%foreach (var dt in Model.DayTotals)
                  {%>
                <th style="width: 30px">
                    <%=new DateTime(dt.Year, dt.Month, dt.Day).ToString("ddd")%><br />
                    <%=new DateTime(dt.Year, dt.Month, dt.Day).ToString("dd")%><br />
                    <%=new DateTime(dt.Year, dt.Month, dt.Day).ToString("MMM")%>
                </th>
                <%}%>
                <th style="width: 40px">Total </th>
            </tr>

            <!-- Rows -->
            <%foreach (var ts in Model.TimeSheets)
              {%>
            <tr>
                <td>
                    <%if (ts.editStatus)
                      { %>
                    <a href="<%=HttpRuntime.AppDomainAppVirtualPath.TrimEnd('/') %>/Timesheet/edit/<%=Html.Encode(ts.Resource_ID) %>/Date/<%=Html.Encode(Model.Date.ToString("dd-MMM-yyyy")) %>/User/<%=Html.Encode(Model.User_CAI.ToString()) %>">
                        <img src="<%=HttpRuntime.AppDomainAppVirtualPath.TrimEnd('/') %>/images/edit-16x16.png" title="Edit" alt="Edit" /></a>
                    <%}%>
                </td>
                <!-- Controls -->
                <td>
                    <%=Html.Encode(ts.Status)%>
                </td>
                <!-- Controls -->
                <td>
                    <%=Html.Encode(ts.Project_Number)%>
                </td>
                <!-- Controls -->
                <td>
                    <%=Html.Encode(ts.SAP)%>
                </td>
                <!-- Controls -->
                <td>
                    <%=Html.Encode(ts.WBS)%>
                </td>
                <!-- Controls -->
                <td>
                    <%=Html.Encode(ts.Project_Name)%>
                </td>
                <!-- Controls -->
                <!-- Days -->
                <%foreach (var d in ts.Days)
                  { %>
                <td>
                    <span class="d<%=Html.Encode(d.Day.ToString())  %>">
                        <%=Html.Encode((!d.HoursWorked.HasValue) ? string.Empty : d.HoursWorked.Value.ToString("#0.00"))%>
                    </span>
                </td>
                <%}%>
                <td style="text-align: right">
                    <%=Html.Encode(ts.Total.ToString("#0.00") ?? string.Empty)%></td>
            </tr>
            <%} %>
            <!-- Totals -->
            <tr style="background-color: #f5f5dc">
                <td></td>
                <td></td>
                <td></td>
                <td></td>
                <td></td>
                <td></td>
                <%foreach (var d in Model.DayTotals)
                  {%>
                <td>
                <span id="<%="t"+ d.Day.ToString() %>">
                    <%=Html.Encode((d.HoursWorked == 0) ? string.Empty : d.HoursWorked.Value.ToString("#0.00"))%>
                    </span>
                </td>
                <%}%>
                <td style="text-align: right">
                <span id="grandtotal">
                    <%=Html.Encode(Model.GrandTotal.ToString("#0.00"))%>
                    </span>
                </td>
            </tr>
            
                        <!--Saco Row-->
            <tr style="background-color: #009DD9; color: White">
                <td colspan="6" style="font-weight:bold">Saco</td>
                <%foreach (var d in Model.DaySacoTotals)
                  {%>
                <td>
                   <span id="<%="s"+ d.Day.ToString() %>">
                    <%=Html.Encode((d.HoursWorked == 0) ? string.Empty : d.HoursWorked.Value.ToString("#0.00"))%>
                    </span>
                </td>
                <%}%>
                <td>
                <span id="grandsacototal">
                    <%=Html.Encode((Model.DaySacoTotals.Sum(s => s.HoursWorked) == 0) ? string.Empty : Model.DaySacoTotals.Sum(s => s.HoursWorked).Value.ToString("#0.00"))%>
                    </span>
                </td>

            </tr>
        </table>
    </div>
    <%}
      else
      { %>
    <div>
        No Timesheets Available
    </div>
    <%} %>    

    <%if (((List<TSDTReports.Models.tbl_Resource>)ViewData["UserProjects"]).Count > 0)
      { %>
    <div id="popupContact" style="overflow: scroll; padding: 5px;">
        <table class="Control" style="background-color: white; border-collapse: collapse;" border="1" rules="all" cellspacing="0" cellpadding="3">
        <tr><th>Hide</th><th>MOC</th><th>WBS</th><th>Project Name</th></tr>
        <%foreach (var item in (List<TSDTReports.Models.tbl_Resource>)ViewData["UserProjects"])
          { %>
          <tr>
          <td><%=Html.CheckBox("chk" + item.Resource_ID, item.Hide)%></td>
          <td><%=Html.Encode(item.tbl_Project.Project_Number.ToString())%></td>
          <td><%=Html.Encode(item.tbl_Project.WBS)%></td>
          <td><%=Html.Encode(item.tbl_Project.Project_Name)%></td>
          
          
          </tr>
        <%} %>
        </table>
        <input type="submit" name="submitbutton" id="UpdateProjects" value ="Update" />
        <input type="button" id="btnUpdateCancel" value ="Cancel" />

    </div>
    <%} %>
    <div id="backgroundPopup">
    </div>
    <%} %>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="headContent" runat="server">

    <script src="<%=HttpRuntime.AppDomainAppVirtualPath.TrimEnd('/') %>/Scripts/popup.js"  type="text/javascript"></script>
    <style type="text/css">
        .Control th, .footer
        {
            background-color: #f5f5dc;
        }
        input.Edit
        {
            width: 30px;
            font-size: 8pt;
        }
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
            z-index: 1;
        }
        #popupContact
        {
            display: none;
            position: fixed;
            _position: absolute; /* hack for internet explorer 6*/
            height: 384px;
            width: 448px;
            background: #FFFFFF;
            border: 2px solid #cecece;
            z-index: 2;
            padding: 12px;
            font-size: 13px;
        }
    </style>

    <script type="text/javascript">

        function checktotals() {
            var sacolist = $("span[id^=s]");
            var totallist = $("span[id^=t]");
            var sacoitem = 0;
            var totalitem = 0;
            var totalsindex = 0;
            $.each(totallist, function(i, n) {
                totalsindex = i + 1;
                sacoitem = parseFloat($("#s" + totalsindex).text());
                totalitem = parseFloat($("#t" + totalsindex).text());
                if (isNaN(sacoitem)) sacoitem = 0;

                try {
                    if (totalitem > sacoitem) {

                        $("#t" + totalsindex).css({ "color": "red", "font-weight": "bold" });
                    }
                    else
                        $("#t" + totalsindex).css({ "color": "black", "font-weight": "normal" });
                }
                catch (err) {
                }

            });
            
            try {
                if (parseFloat($("#grandtotal").text()) > parseFloat($("#grandsacototal").text()))
                    $("#grandtotal").css({ "color": "red", "font-weight": "bold" });
                else
                    $("#grandtotal").css({ "color": "black", "font-weight": "normal" });
            }
            catch (err) {
            }
        }


        $(function() {
        checktotals();
            $("#btnViewProjects").click(function() {
            loadPopup("popupContact");
            centerPopup("popupContact");


        });

        $("#btnUpdateCancel").click(function() {
            disablePopup("popupContact");
        });

        //Click out event!
        $("#backgroundPopup").click(function() {
        disablePopup("popupContact");
        });

            //Press Escape event!
            $(document).keypress(function(e) {
                if (e.keyCode == 27 && popupStatus == 1) {
                    disablePopup("popupContact");
                }
            });

            $('#txtDate').datepicker({ dateFormat: 'dd-M-yy', duration: 'fast' });

            $("#ddlUsers").change(function() {
                window.document.forms[0].submit();
            });

            $('#txtDate').change(function() {
                window.document.forms[0].submit();
            });

            $("#popupContact").draggable();

        });
    </script>

</asp:Content>
