$().ready(function() {
    $("#tabs > ul").tabs();
    $('input[id$="Date"]').attachDatepicker({ dateFormat: 'dd-M-yy', speed: 'fast' });
   // FillSelect("http://zarfntvcisdev1/webservicestsdt/tsdt.svc/projects", "ddlProjects", "Please Select Project", "Projects", "Project_ID", "Project_Number", "Project_Name", "-");
   // FillSelect("http://zarfntvcisdev1/webservicestsdt/tsdt.svc/users", "ddlUsers", "Please Select User", "Users", "User_CAI", "User_FirstName", "User_LastName", " ");
//    $('#btnProjectReport').click(function() {
//    $.getJSON("http://zarfntvcisdev1/webservicestsdt/tsdt.svc/project/userhoursforproject/?id=" + $("#ddlProjects").val() + "&start=" + $("#txtStartDate").val() + "&end=" + $("#txtEndDate").val(),
//                    function(data) {
//                        if (data.length == 0)
//                            $('#Container').html("No data for this period");
//                        else
//                            BuildProjectReportTable(data);
//                    });

//    });

//    $("#btnUserReport").click(function() {
//    $.getJSON("http://zarfntvcisdev1/webservicestsdt/tsdt.svc/user/projecthours/?CAI=" + $("#ddlUsers").val() + "&start=" + $("#txtUserStartDate").val() + "&end=" + $("#txtUserEndDate").val(),
//                    function(data) {
//                        if (data.length == 0)
//                            $('#Container').html("No data for this period");
//                        else
//                            BuildUserReportTable(data);
//                    });
//    });
});

function GetChartData2(data, legendlabel, valuelabel) {
    var chartdata = new Array();
    var counter = 0.5;
    for (var d in data) {
        chartdata.push({ label: data[d][legendlabel], data: [[counter, 0], [counter, data[d][valuelabel]]], bars: { show: true} });
        counter++;
    }

    return chartdata;
}

function GetxTicks2(data, xLabel) {
    var xticks = new Array();
    xticks.push([0, ""]);
    var tick_counter = 1;
    for (var d in data) {
        xticks.push([tick_counter, data[d][xLabel]]);
        tick_counter++;
    }
    return xticks;
}

function FillSelect(ServiceURL, controlID, ddlMessage, JsonColl, Value, FirstTextField, SecondTextField, separator) {
    $("#" + controlID).after("<span id='ajaximg'>Please Wait...</span>");
    $.getJSON(ServiceURL,
            function(data) {
                var options = "<option value='0'>" + ddlMessage + "</option>";
                for (var i = 0; i < data[JsonColl].length; i++) {
                    options += '<option value="' + data[JsonColl][i][Value] + '" >' +
                                    data[JsonColl][i][FirstTextField] + separator + data[JsonColl][i][SecondTextField] +
                                '</option>';
                }
                $("#" + controlID).html(options);
                $("#ajaximg").remove();
            });
}

function BuildUserReportTable(data) {
    var details = "<ul>" +
                                    "<li>" +
                                        "<label>User CAI: <label>" + $("#ddlUsers").val() +
                                    "</li>" +
                                   "<li>" +
                                        "<label>User Name: </label>" + $("#ddlUsers option:selected").text() +
                                    "</li>" +
                                "</ul>";

    var table = '<table id="dataTable" class="tablesorter"><thead><tr><th>ID</th><th>Project Number</th><th>Project Name</th><th>Hours</th></tr></thead><tbody>';
    var totalHours = 0;
    for (var d in data) {
        var row = '<tr>';

        row += '<td>' + data[d].ProjectID + '</td>';
        row += '<td>' + data[d].ProjectNumber + '</td>';
        row += '<td>' + data[d].ProjectName + '</td>';
        row += '<td style="text-align:right">' + data[d].Hours + '</td>';

        row += '</tr>';
        totalHours += data[d].Hours;
        table += row;
    }

    table += '<tfoot><tr><th colspan="3">Total</th><th style="text-align:right">' + totalHours.toFixed(2) + '</th></tr></tfoot></tbody></table>';

    $('#Container').html(details + table);
    $("#dataTable").tablesorter();

    //            xax = { min: 0, max: data["Months"].length + 1, ticks: GetxTicks2(data["Months"], "Month_Year") };
    //            options = { xaxis: xax };
    //            $.plot($("#myChart"), GetChartData2(data["Months"], "Month_Year", "Hours"), options);
};

function BuildProjectReportTable(data) {
    var details = "<ul>" +
                                    "<li>" +
                                        "<label>Project Name: <label>" + data["Project_Name"] +
                                    "</li>" +
                                   "<li>" +
                                        "<label>Project Number: </label>" + data["Project_Number"] +
                                    "</li>" +
                                "</ul>";

    var table = '<table id="dataTable" class="tablesorter"><thead><tr><th>First Name</th><th>Last Name</th><th>CAI</th><th>Hours</th></tr></thead><tbody>';
    var totalHours = 0;
    for (var d in data["Users"]) {
        var row = '<tr>';

        row += '<td>' + data["Users"][d].FirstName + '</td>';
        row += '<td>' + data["Users"][d].LastName + '</td>';
        row += '<td>' + data["Users"][d].CAI + '</td>';
        row += '<td style="text-align:right">' + data["Users"][d].hours + '</td>';

        row += '</tr>';
        totalHours += data["Users"][d].hours;
        table += row;
    }

    table += '<tfoot><tr><th colspan="3">Total</th><th style="text-align:right">' + totalHours.toFixed(2) + '</th></tr></tfoot></tbody></table>';

    $('#Container').html(details + table);
    $("#dataTable").tablesorter();

    //            xax = { min: 0, max: data["Months"].length + 1, ticks: GetxTicks2(data["Months"], "Month_Year") };
    //            options = { xaxis: xax };
    //            $.plot($("#myChart"), GetChartData2(data["Months"], "Month_Year", "Hours"), options);
};

