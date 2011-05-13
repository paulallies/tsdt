<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<TSDTReports.Models.ReportPackage>" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>ExcelReport</title>
</head>
<body>
 <table class="Control" id="tblTimeSheets" style="background-color: white; border-collapse: collapse;" border="1" rules="all" cellspacing="0" cellpadding="3">
            <!-- Header -->
            <tr style="background-color:#f5f5dc">

                <th style="width: 50px">
                    Status
                </th>
                <th style="width:50px">
                    MOC
                </th>
                <th style="width:100px">
                    SAP
                </th>
                <th style="width : 130px">
                    Cost Code
                </th>
                <th style="width : 300px">
                Project Name
                   
                </th>
                <%for (DateTime dt = Model.DateFrom; dt < Model.DateTo.AddDays(1); dt = dt.AddDays(1))
                  {%>
                <th style="width:50px">
                    <%=new DateTime(dt.Year, dt.Month, dt.Day).ToString("ddd")%><br/>
                    <%=new DateTime(dt.Year, dt.Month, dt.Day).ToString("dd")%><br/>
                    <%=new DateTime(dt.Year, dt.Month, dt.Day).ToString("MMM")%>
                </th>
                <%}%>
                <th style="width:40px">
                    Total
                </th>
            </tr>
            
            <!-- Rows -->
            <%foreach (var ts in Model.TimeSheets)
              {%>
              <tr >    
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
                    <td><span class="d<%=Html.Encode(d.Day.ToString())  %>">
                       <%=Html.Encode((!d.HoursWorked.HasValue) ? string.Empty : d.HoursWorked.Value.ToString("#0.00")) %>
                       </span>
                    </td>
                <%}%>
           

                <td style="text-align:right"><%=Html.Encode(ts.Total.ToString() ?? string.Empty)  %></td>
            </tr>
            <%} %>
            
            <tr style="background-color:#f5f5dc">

                <td >
                 
                </td>
                <td >
                 
                </td>
                <td >
                    
                </td>
                <td >
                    
                </td>
                <td  width="300">
                    
                </td>
                <%foreach(var d in Model.DayTotals)
                  {%>
                <td>
                  <%=Html.Encode((d.HoursWorked==0) ? string.Empty : d.HoursWorked.Value.ToString("#0.00"))%>
                </td>
                <%}%>
                <td style="text-align:right">
                   <%=Html.Encode(Model.GrandTotal.ToString("#0.00")) %>
                </td>
            </tr>
            </table>
</body>
</html>
