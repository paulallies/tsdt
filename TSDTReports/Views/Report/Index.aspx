<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<TSDTReports.Models.ReportPackage>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Index
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <% using (Html.BeginForm("index", "Report", FormMethod.Post))
       { %>
    <table>
        <tr>
            <td>User: </td>
            <td>
                <%if ((string)ViewData["Role"] == "Administrator")
                  {%>
                <%=Html.DropDownList("ddlUsers", (SelectList)ViewData["Users"], "Select", new { style = "font-size: 8pt" })%>
                <%}
                  else
                  { %>
                <%=Html.TextBox("ddlUsers", Model.User_CAI, new { style = "font-size: 8pt", @readonly="readonly" })%>
                <%} %>
            </td>
            <td>Date From: </td>
            <td>
                <%=Html.TextBox("txtDateFrom", Model.DateFrom.ToString("dd-MMM-yyyy"), new { style = "font-size: 8pt" })%>
            </td>
            <td>Date To: </td>
            <td>
                <%=Html.TextBox("txtDateTo", Model.DateTo.ToString("dd-MMM-yyyy"), new { style = "font-size: 8pt" })%>
            </td>
            <td>
                <input type="submit" value="Get Report" name="submit" />
            </td>
            <%if (Model.TimeSheets.Count > 0)
              { %>
            <%--              <td>
                <input type="submit" value="Send to Excel" name="submit" id="btnSendToExcel" />
            </td>--%>
            <td>
                <input type="submit" value="Print" name="submit" id="btnPrint" />
            </td>
            <%} %>
        </tr>
    </table>
    <%} %>
    <br />
    <%if (Model.TimeSheets.Count > 0)
      { %>
    <div style="overflow: scroll; padding: 5px;">
        <table class="Control" id="tblTimeSheets" style="background-color: white; max-width: 2200px; border-collapse: collapse;"
            border="1" rules="all" cellspacing="0" cellpadding="3">
            <!-- Header -->
            <tr>
                <th style="width: 50px">Status </th>
                <th style="width: 50px">Project&nbsp;No.</th>
                <th style="width: 100px">SAP </th>
                <th style="width: 130px">Cost Code </th>
                <th style="width: 300px;">Project Name </th>
                <%for (DateTime dt = Model.DateFrom; dt < Model.DateTo.AddDays(1); dt = dt.AddDays(1))
                  {%>
                <th style="width: 30px">
                    <%=new DateTime(dt.Year, dt.Month, dt.Day).ToString("ddd")%><br />
                    <%=new DateTime(dt.Year, dt.Month, dt.Day).ToString("dd")%><br />
                    <%=new DateTime(dt.Year, dt.Month, dt.Day).ToString("MMM")%>
                </th>
                <%}%>
                <th style="width: 40px">Mth Total </th>
                <th style="width: 40px">Total </th>
                <th style="width: 40px">Prj Hours</th>
            </tr>
            <!-- Rows -->
            <%foreach (var ts in Model.TimeSheets)
              {%>
            <tr>
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
                <td style="text-align: right">
                    <span class="d<%=Html.Encode(d.Day.ToString())  %>">
                        <%=Html.Encode((!d.HoursWorked.HasValue) ? string.Empty : d.HoursWorked.Value.ToString("#0.00"))%>
                    </span>
                </td>
                <%}%>
                <td style="text-align: right; font-weight:bold">
                    <%=Html.Encode(ts.MonthTotal.ToString("#0.00") ?? string.Empty)%></td>
                <td style="text-align: right; background-color: #f5f5dc">
                    <%if (ts.SAP != "Leave")
                      { %>
                    <%=Html.Encode(ts.Total.ToString("#0.00") ?? string.Empty)%>
                    <%} %></td>
                <td style="text-align: right;background-color: #f5f5dc">
                    <%=Html.Encode((ts.ProjectHours == 0) ? string.Empty : ts.ProjectHours.ToString("#0.00"))%>
                </td>
            </tr>
            <%} %>
            <!-- Totals -->
            <tr style="background-color: #f5f5dc">
                <td colspan="5" style="text-align: right; font-weight: bold">Totals</td>
                <%foreach (var d in Model.DayTotals)
                  {%>
                <td style="text-align: right">
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
                <td></td>
                <td></td>
            </tr>
            <!--Saco Row-->
            <tr style="background-color: #009DD9; color: White">
                <td colspan="5" style="font-weight: bold; text-align: right">Saco Hours</td>
                <%foreach (var d in Model.DaySacoTotals)
                  {%>
                <td style="text-align: right">
                    <span id="<%="s"+ d.Number.ToString() %>">
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
            <tr>
                <td colspan="5" style="font-weight: bold; text-align: right">Diff</td>
                <%
                    decimal diff = 0M;
                    decimal diffTotal = SacoGrandtotal - Model.GrandTotal;
                    for (int i = 0; i < Model.DaySacoTotals.Count; i++)
                    {
                        if (Model.DayTotals[i].HoursWorked.HasValue && Model.DaySacoTotals[i].HoursWorked.HasValue)
                            diff = Model.DaySacoTotals[i].HoursWorked.Value - Model.DayTotals[i].HoursWorked.Value;
                %>
                <td style="text-align: right; ">
                    <span id="<%="diff"+ (i+1).ToString() %>" style="font-weight:bold">
                        <%=(diff == 0) ? string.Empty : diff.ToString("#0.00")%>
                    </span>
                </td>
                <%}%>
                <td style="text-align: right">
                    <span id="diffTotal" style="font-weight: bold">
                        <%=diffTotal.ToString("#0.00")%>
                    </span>
                </td>
                <td></td>
                <td></td>
            </tr>
        </table>
    </div>
    <%}
      else
      { %>
    No TimeSheets Available
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
            $('#txtDateFrom').datepicker({ dateFormat: 'dd-M-yy', duration: 'fast' });
            $('#txtDateTo').datepicker({ dateFormat: 'dd-M-yy', duration: 'fast' });

            $("input[id^=txtDate]").change(function() {
                ChangeStatus(false);

            });

            $("#ddlUsers").change(function() {
                ChangeStatus(false);
            });



            $("input[name=submit]").click(function() {
                var dateto = $('#txtDateTo').datepicker("getDate");
                var datefrom = $('#txtDateFrom').datepicker("getDate");
                var diff = dateto - datefrom;
                if (diff / (1000 * 60 * 60 * 24) <= 31) {
                    ChangeStatus(true);
                    return true;
                }
                else {
                    ChangeStatus(false);
                    alert("Range must be 31 days and less");
                    return false;
                }
            });

        });

        function ChangeStatus(status) {
            if (status == true) {
                $("#btnSendToExcel").show();
                $("#btnPrint").show();
            }
            else {
                $("#btnSendToExcel").hide();
                $("#btnPrint").hide();
            }
        }

        
    </script>

</asp:Content>
