<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<TSDTReports.Models.TimeSheetPackage>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Edit TimeSheet
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <% using (Html.BeginForm("Update", "Timesheet", FormMethod.Post))
       { %>
    <table>
        <tr>
            <td>User: </td>
            <td>
                <%=Html.DropDownList("ddlUsers", (SelectList)ViewData["Users"], "Select")%>
            </td>
            <td>Date: </td>
            <td>
                <%=Html.TextBox("txtDate", Model.Date.ToString("dd-MMM-yyyy"))%>
            </td>
        </tr>
    </table>
    <br />
    <div style="overflow: scroll; padding: 5px;">
        <table class="Control" id="tblTimeSheets" style="background-color: white; width: 2000px;
            border-collapse: collapse;" border="1" rules="all" cellspacing="0" cellpadding="3">
            <!-- Header -->
            <tr>
                <th style="width: 50px"></th>
                <th style="width: 50px">Status </th>
                <th style="width: 50px">MOC </th>
                <th style="width: 100px">SAP </th>
                <th style="width: 130px">Cost Code </th>
                <th style="width: 300px">Project Name </th>
                <%foreach (var dt in Model.DayTotals)
                  {%>
                <th style="width: 30px">
                    <%=new DateTime(dt.Year, dt.Month, dt.Day).ToString("ddd") %><br />
                    <%=new DateTime(dt.Year, dt.Month, dt.Day).ToString("dd") %><br />
                    <%=new DateTime(dt.Year, dt.Month, dt.Day).ToString("MMM") %>
                </th>
                <%}%>
                <th style="width: 40px">Month Total </th>
                <th style="width: 40px">Total </th>
                <th style="width: 40px">Project Hours</th>
            </tr>
            <!-- Rows -->
            <%foreach (var ts in Model.TimeSheets)
              {%>
            <%if (ts.Resource_ID == (int)ViewData["Resource_id"])
              { %>
            <tr style="background-color: #f5f5dc">
                <%}
              else
              { %>
                <tr>
                    <%} %>
                    <td>
                        <%if (ts.Status != "CLSD")
                          { %>
                        <%if (ts.Resource_ID == (int)ViewData["Resource_id"])
                          { %>
                        <a href="#" id="<%="update" + Html.Encode(ts.Project_ID) %>" onclick="TSDT.jsubmit()">
                            <img src="<%=HttpRuntime.AppDomainAppVirtualPath.TrimEnd('/') %>/images/check-16x16.png"
                                title="Update" alt="Update" /></a> <a href="<%=HttpRuntime.AppDomainAppVirtualPath.TrimEnd('/') %>/Timesheet/Cancel/Date/<%=Html.Encode(Model.Date.ToString("dd-MMM-yyyy")) %>/User/<%=Html.Encode(Model.User_CAI.ToString()) %>">
                                    <img src="<%=HttpRuntime.AppDomainAppVirtualPath.TrimEnd('/') %>/images/undo-16x16.png"
                                        title="Cancel" alt="Cancel" /></a>
                        <%} %>
                        <%} %>
                    </td>
                    <!-- Controls -->
                    <td>
                        <%=Html.Encode(ts.Status) %>
                    </td>
                    <!-- Controls -->
                    <td>
                        <%=Html.Encode(ts.Project_Number) %>
                    </td>
                    <!-- Controls -->
                    <td>
                        <%=Html.Encode(ts.SAP) %>
                    </td>
                    <!-- Controls -->
                    <td>
                        <%=Html.Encode(ts.WBS) %>
                    </td>
                    <!-- Controls -->
                    <td>
                        <%=Html.Encode(ts.Project_Name) %>
                    </td>
                    <!-- Controls -->
                    <!-- Days -->
                    <%foreach (var d in ts.Days)
                      { %>
                    <td style="text-align: right">
                        <%if (ts.Resource_ID == (int)ViewData["Resource_id"])
                          { %>
                        <%=Html.TextBox("dayentry|" + d.Day.ToString() + "|" + ts.Project_ID.ToString(), d.HoursWorked.ToString() ?? string.Empty, new { @class = "Edit" , @maxlength="5" })%>
                        <%}
                          else
                          { %>
                        <%=Html.Encode(d.HoursWorked.ToString() ?? string.Empty)%>
                        <%} %>
                    </td>
                    <%}%>
                    <td style="text-align: right">
                        <%=Html.Encode(ts.MonthTotal.ToString("#0.00") ?? string.Empty)%></td>
                    <td style="text-align: right">
                    <%if (ts.SAP != "Leave")
                      { %>
                        <span id="<%="total" + ts.Project_ID %>">
                            <%=Html.Encode(ts.Total.ToString("#0.00") ?? string.Empty)%>
                        </span>
                        <%} %>
                    </td>
                    <td style="text-align: right">
                        <span id="<%="ph" + ts.Project_ID %>">
                            <%=Html.Encode((ts.ProjectHours == 0) ? string.Empty : ts.ProjectHours.ToString("#0.00"))%>
                        </span>
                    </td>
                </tr>
                <%} %>
                <tr style="background-color: #f5f5dc;">
                    <td colspan="6" style="font-weight: bold; text-align: right">Totals</td>
                    <%foreach (var d in Model.DayTotals)
                      {%>
                    <td style="text-align: right">
                        <span id="<%="t"+ d.Day.ToString() %>">
                            <%=Html.Encode((d.HoursWorked==0) ? string.Empty : d.HoursWorked.Value.ToString("#0.00"))%>
                        </span>
                    </td>
                    <%}%>
                    <td style="text-align: right">
                        <%=Html.Encode(Model.GrandTotal.ToString("#0.00")) %>
                    </td>
                    <td></td>
                    <td></td>
                </tr>
                <!--Saco Row-->
                <tr style="background-color: #009DD9; color: White">
                    <td colspan="6" style="font-weight: bold; text-align: right">Saco Hours</td>
                    <%foreach (var d in Model.DaySacoTotals)
                      {%>
                    <td style="text-align: right">
                        <span id="<%="s"+ d.Day.ToString() %>">
                            <%=Html.Encode((d.HoursWorked == 0) ? string.Empty : d.HoursWorked.Value.ToString("#0.00"))%>
                        </span>
                    </td>
                    <%}%>
                    <td style="text-align: right">
                        <span id="grandsacototal">
                            <%  decimal SacoGrandtotal = 0M;
                                if (Model.DaySacoTotals.Sum(s => s.HoursWorked).HasValue)
                                    SacoGrandtotal = Model.DaySacoTotals.Sum(s => s.HoursWorked).Value;
                            %>
                            <%=(SacoGrandtotal == 0)? string.Empty : SacoGrandtotal.ToString("#0.00")%>
                        </span>
                    </td>
                    <td></td>
                    <td></td>
                </tr>
                <!--Diff Row-->
                <tr style="background-color: #f5f5dc;">
                    <td colspan="6" style="font-weight: bold; text-align: right">Diff</td>
                    <%
                        decimal diff = 0M;
                        decimal diffTotal = SacoGrandtotal - Model.GrandTotal;
                        for (int i = 0; i < Model.DaySacoTotals.Count; i++)
                        {
                            if (Model.DayTotals[i].HoursWorked.HasValue && Model.DaySacoTotals[i].HoursWorked.HasValue)
                                diff = Model.DaySacoTotals[i].HoursWorked.Value - Model.DayTotals[i].HoursWorked.Value;
                    %>
                    <td style="text-align: right">
                        <span id="<%="diff"+ (i+1).ToString() %>" style="font-size:7pt">
                            <%=(diff == 0) ? string.Empty : diff.ToString("#0.00")%>
                        </span>
                    </td>
                    <%}%>
                    <td style="text-align: right">
                        <span id="diffTotal">
                            <%=diffTotal.ToString("#0.00")%>
                        </span>
                    </td>
                    <td></td>
                    <td></td>
                </tr>
        </table>
    </div>
    <input type="hidden" value='<%=ViewData["Resource_id"].ToString() %>' name="resid"
        id="resid" />
    <%} %>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="headContent" runat="server">
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
    </style>

    <script type="text/javascript">

        //Global Variables
        var TSDT = {};     
        TSDT.urlPath = helper.rtrim('<%=ResolveUrl("~/")%>', "/");
        TSDT.currentval = 0;
        TSDT.currentColTotal = 0;
        TSDT.currentRowTotal = 0;
        TSDT.colIndex = 0;
        TSDT.projectIndex = 0;
        TSDT.status = true;

        //Functions
        TSDT.GetIndex = function(inputControl, index) {
            var IndexArray = inputControl.split("|");
            return IndexArray[index];
        }


        TSDT.checkTotals = function() {
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
                    if (totalitem > sacoitem)
                        $("#t" + totalsindex).css({ "color": "red", "font-weight": "bold" });
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



       TSDT.submit1 =  function() {
            window.location.href = TSDT.urlPath + "/TimeSheet/Index?ddlUsers=" + $("#ddlUsers").val() + "&txtDate=" + $("#txtDate").val();
        }
        
        TSDT.jsubmit = function() {
            if (TSDT.status == false) 
                alert("Hours booked exceed limit");
            else 
                window.document.forms[0].submit();
        }

        $(function() {
            TSDT.checkTotals();
            Array.prototype.indexOf = function(val) {
                var i = this.length;
                while (this[--i] != val)
                    if (i == 0) return -1;
                return i;
            }

            $("input[id^=dayentry]").keydown(function(event) {
                var arrayIndex = helper.keyArray.indexOf(event.keyCode);
                if (arrayIndex > -1)
                    return true;
                else
                    return false;
            });

            $("input[id^=dayentry]").focus(function() {

                TSDT.colIndex = TSDT.GetIndex($(this).attr("id"), 1);
                TSDT.projectIndex = TSDT.GetIndex($(this).attr("id"), 2);
                TSDT.currentColTotal = 0;
                TSDT.currentval = 0;
                TSDT.currentRowTotal = 0;
                //Get the current value of this control
                if ($.trim($(this).val()).length > 0) {
                    TSDT.currentval = parseFloat($(this).val());
                }
                //Get the current col total
                if ($.trim($("#t" + TSDT.colIndex).text()).length > 0) {
                    TSDT.currentColTotal = parseFloat($("#t" + TSDT.colIndex).text());
                }
                //Get the current row total
                if ($.trim($("#total" + TSDT.projectIndex).text()).length > 0) {
                    TSDT.currentRowTotal = parseFloat($("#total" + TSDT.projectIndex).text());
                }


            });

            $("input[id^=dayentry]").blur(function() {
                var newEntry = 0;
                //Get the new value entered by the user
                if ($.trim($(this).val()).length > 0) {
                    newEntry = $(this).val();
                }

                //get the project Hours for this row
                var projectHours = 0;
                if ($("#ph" + TSDT.projectIndex).text().length > 0)
                    projectHours = $("#ph" + TSDT.projectIndex).text();
                //Calculate total hours worked for this project
                TSDT.currentRowTotal = TSDT.currentRowTotal - TSDT.currentval + parseFloat(newEntry);
                //Update the row total for this project
                if (TSDT.currentRowTotal > 0)
                    $("#total" + TSDT.projectIndex).text(TSDT.currentRowTotal.toFixed(2));

                else
                    $("#total" + TSDT.projectIndex).text("");


                //Hide update button and alert User
                if ((TSDT.currentRowTotal > projectHours) && (projectHours > 0)) {
                    $("#total" + TSDT.projectIndex).css({ "color": "red", "font-weight": "bold" });
                    TSDT.status = false;
                }
                else {
                    $("#total" + TSDT.projectIndex).css({ "color": "black", "font-weight": "normal" });
                    TSDT.status = true;
                }

                //get the saco hours for this day
                var sacototal = $("#s" + TSDT.colIndex).text();
                //Calculate new Col Total
                TSDT.currentColTotal = TSDT.currentColTotal - TSDT.currentval + parseFloat(newEntry);
                //Update the total cell to this day
                if (TSDT.currentColTotal > 0) {
                    var diff = sacototal - TSDT.currentColTotal;
                    $("#diff" + TSDT.colIndex).text(diff.toFixed(2));
                    $("#t" + TSDT.colIndex).text(TSDT.currentColTotal.toFixed(2));
                }
                else
                    $("#t" + TSDT.colIndex).text("");
                //Style the cells accordingly   



                TSDT.checkTotals();

            });


            $('#txtDate').datepicker({ dateFormat: 'dd-M-yy', speed: 'fast' });

            $("#ddlUsers").change(function() {
                TSDT.submit1();
            });

            $('#txtDate').change(function() {
                TSDT.submit1();
            });
        });


    </script>

</asp:Content>
