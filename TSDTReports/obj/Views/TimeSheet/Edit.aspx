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
                <th>Total </th>
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
                        <a href="#" onclick="jsubmit()">
                            <img src="<%=HttpRuntime.AppDomainAppVirtualPath.TrimEnd('/') %>/images/check-16x16.png" title="Update" alt="Update" /></a> <a href="<%=HttpRuntime.AppDomainAppVirtualPath.TrimEnd('/') %>/Timesheet/Cancel/Date/<%=Html.Encode(Model.Date.ToString("dd-MMM-yyyy")) %>/User/<%=Html.Encode(Model.User_CAI.ToString()) %>">
                                <img src="<%=HttpRuntime.AppDomainAppVirtualPath.TrimEnd('/') %>/images/undo-16x16.png" title="Cancel" alt="Cancel" /></a>
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
                    <td>
                        <%if (ts.Resource_ID == (int)ViewData["Resource_id"])
                          { %>
                        <%=Html.TextBox("dayentry" + d.Day.ToString(), d.HoursWorked.ToString() ?? string.Empty, new { @class = "Edit", @maxlength="5" })%>
                        <%}
                          else
                          { %>
                        <%=Html.Encode(d.HoursWorked.ToString() ?? string.Empty)%>
                        <%} %>
                    </td>
                    <%}%>
                    <td style="text-align: right">
                        <%=Html.Encode(ts.Total.ToString() ?? string.Empty)  %></td>
                </tr>
                <%} %>
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
                            <%=Html.Encode((d.HoursWorked==0) ? string.Empty : d.HoursWorked.Value.ToString("#0.00"))%>
                        </span>
                    </td>
                    <%}%>
                    <td style="text-align: right">
                        <%=Html.Encode(Model.GrandTotal.ToString("#0.00")) %>
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
                    <%=Html.Encode((Model.DaySacoTotals.Sum(s => s.HoursWorked) == 0) ? string.Empty : Model.DaySacoTotals.Sum(s => s.HoursWorked).Value.ToString("#0.00"))%>
                </td>
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
        

    
    
        var currentval = 0;
        var currenttotal = 0;
        var index = 0;
        $(function() {
            checktotals();
            //            $("input[id^=r]").blur(function() {
            //                if ($.trim($(this).val()).length > 0) {
            //                    if (!isValidNumber($.trim($(this).val()))) {
            //                        $(this).css("color", 'red');
            //                    } else {
            //                        if ($(this).val() > 12) { $(this).css("color", 'red'); } else
            //                            $(this).css("color", 'black');
            //                    }
            //                }
            //            });

            $("input[id^=dayentry]").keydown(function(event) {
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

            $("input[id^=dayentry]").focus(function() {
                index = $(this).attr("id").replace("dayentry", "")
                currenttotal = 0;
                currentval = 0;
                if ($.trim($(this).val()).length > 0) {
                    currentval = parseFloat($(this).val());
                }
                if ($.trim($("#t" + index).text()).length > 0) {
                    currenttotal = parseFloat($("#t" + index).text());
                }

            });

            $("input[id^=dayentry]").blur(function() {
                var newEntry = 0;
                if ($.trim($(this).val()).length > 0) {
                    newEntry = $(this).val();
                }
                var sacototal = $("#s" + index).text();
                currenttotal = currenttotal - currentval + parseFloat(newEntry);
                if (currenttotal > 0)
                    $("#t" + index).text(currenttotal.toFixed(2));
                else
                    $("#t" + index).text("");

                checktotals();

            });


            $('#txtDate').datepicker({ dateFormat: 'dd-M-yy', speed: 'fast' });

            $("#ddlUsers").change(function() {
                submit1();
            });

            $('#txtDate').change(function() {
                submit1();
            });
        });


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



        function submit1() {
            alert("<%=HttpRuntime.AppDomainAppVirtualPath.TrimEnd('/') %>");
            window.location.href = "/TimeSheet/Index?ddlUsers=" + $("#ddlUsers").val() + "&txtDate=" + $("#txtDate").val();
        }
        function jsubmit() {
            var status = false;
            $.each($("input[id^=dayentry]"), function() {
                if ($.trim($(this).val()).length > 0 && $(this).css('color') == 'red') { status = true; }
            });
            if (status == true) alert("Errors on Page"); else window.document.forms[0].submit();
        }
    </script>

</asp:Content>
