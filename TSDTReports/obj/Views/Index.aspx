<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Timesheet
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
  <% using (Html.BeginForm())
     { %>
    <table>
        <tr>
            <td>
                User:
            </td>
            <td>
                <%=Html.DropDownList("ddlUsers", (SelectList)ViewData["Users"], "Select")%>
            </td>
            <td>
                Date:
            </td>
            <td>
                <%=Html.TextBox("txtDate", ViewData["Date"])%>
            </td>
        </tr>
    </table>
    <br />
    <div style="overflow: scroll; padding: 5px;">
        <table class="Control" id="tblTimeSheets" style="background-color: white; width: 2000px;
            border-collapse: collapse;" border="1" rules="all" cellspacing="0" cellpadding="3">
            <!-- Header -->
            <tr>
                <th style="width: 50px">
                </th>
                <th style="width: 50px">
                    Status
                </th>
                <th>
                    MOC
                </th>
                <th>
                    SAP
                </th>
                <th>
                    Cost Code
                </th>
                <th style="width: 300px">
                    Project Name
                </th>
                <%for (int i = 1; i < (int)ViewData["MonthDays"]; i++)
                  {%>
                <th>
                    <%=i.ToString("0#") %>
                </th>
                <%}%>
                <th>
                    Total
                </th>
            </tr>
            
            <!-- Rows -->
            <%string Mode = ViewData["Mode"].ToString(); string row =  ViewData["row"].ToString(); %>
            <%foreach (var ts in (List<TSDTReports.Models.timesheet>)ViewData["Times"])
              {%>
               <%
                   if (Mode == "Edit" && row == ts.Resource_ID.ToString())
                   { %>
            <tr style="background-color:#f5f5dc">
            <% }
                   else
                   {%>
               <tr >    
            <%} %>
                <td>
                <%if (Mode == "View")
                  { %>
                      <%if (ts.Status != "CLSD")
                        { %>
                        <input type="submit" name="edit<%=Html.Encode(ts.Resource_ID) %>" id="edit<%=Html.Encode(ts.Resource_ID) %>" value="Edit" />
                        <%--<a href="#" id= onclick="Submit('Edit','<%=Html.Encode(ts.Resource_ID) %>')" ><img src="../../images/icon_edit.gif" title="Edit" alt="Edit" /></a>--%>
                       <%} %>
                   <%} %>
                   
                   <%
                   if (Mode == "Edit" && row == ts.Resource_ID.ToString() )
                     { %>
                        <input type="submit" name="update<%=Html.Encode(ts.Resource_ID) %>" id="update<%=Html.Encode(ts.Resource_ID) %>" value="Update" />
                        <input type="submit" name="cancel<%=Html.Encode(ts.Resource_ID) %>" id="cancel<%=Html.Encode(ts.Resource_ID) %>" value="Cancel" />

                      <%--<a href="#" id="update<%=Html.Encode(ts.Resource_ID) %>" onclick='Submit("View","<%=Html.Encode(ts.Resource_ID) %>")' ><img src="../../images/icon_accept.gif" title="Update" alt="Update" /></a>--%>
                      <%--<a href="#" id="cancel<%=Html.Encode(ts.Resource_ID) %>" onclick='Submit("View","<%=Html.Encode(ts.Resource_ID) %>")' ><img src="../../images/icon_undo.gif" title="Cancel" alt="Cancel" /></a>--%>

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
                    <%if (Mode == "View")
                      { %>
                       <%=Html.Encode(d.HoursWorked.ToString() ?? string.Empty) %>
                       <%} else if (Mode == "Edit" && row == ts.Resource_ID.ToString()){ %>
                     
                       <%=Html.TextBox("r" + ts.Resource_ID.ToString() + "d" + d.Day.ToString(), d.HoursWorked.ToString() ?? string.Empty, new { @class = "Edit" })%>
                       <%}else { %>
                           <%=Html.Encode(d.HoursWorked.ToString() ?? string.Empty) %>
                       <%} %>
                    </td>
                <%}%>
           

                <td style="text-align:right"><%=Html.Encode(ts.Total.ToString() ?? string.Empty)  %></td>
            </tr>
            <%} %>
            </table>
            </div>
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
        $(function() {
 
            $('#txtDate').attachDatepicker({ dateFormat: 'dd-M-yy', speed: 'fast' });

            $("#ddlUsers").change(function() {
                window.document.forms[0].submit();
            });

            $('#txtDate').change(function() {
                window.document.forms[0].submit();
            });
        });
    </script>
</asp:Content>
