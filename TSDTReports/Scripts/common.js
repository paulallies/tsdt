
var helper = {};

helper.DayNames = new Array("Mon", "Tue", "Wed", "Thu", "Fri", "Sat", "Sun");
helper.keyArray = [190, 8, 37, 39, 46, 110, 9, 48, 49, 40,50, 51, 52, 53, 54, 55, 56, 57, 58, 95, 96, 97, 98, 99, 100, 101, 102, 103, 104, 105];


/****************************** 
** Common Javascript routines
** Simon Mugan: Sep 2000
*******************************/

//Needs jquery
helper.FillSelect =  function(selectControl, Data) {
    var drp = document.getElementById(selectControl);
    $.each(Data, function(ky, val) 
    {
     drp.options[drp.options.length] = new Option(ky, val);
    });

}


helper.trim = function(str, chars) {
    return helper.ltrim(helper.rtrim(str, chars), chars);
}

helper.ltrim = function(str, chars) {
    chars = chars || "\\s";
    return str.replace(new RegExp("^[" + chars + "]+", "g"), "");
}

helper.rtrim =  function(str, chars) {
    chars = chars || "\\s";
    return str.replace(new RegExp("[" + chars + "]+$", "g"), "");
}

helper.verifyDateElement = function(control, validatorname) {
    if ($(control).val() != "") 
    {
           if (isValidDate($(control).val(), false) == false) {
               if ($('span[id=' + validatorname + ']').length == 0) {
                   $(control).after("<span style='color:red' id=" + validatorname + ">Invalid Date</span>");
               }
               return false;
        }
        else {
            $('span[id=' + validatorname + ']').remove()
            return true;
        }
    }

}

helper.verifyNumber = function(control, validatorname) {
    if ($(control).val() != "") {
        if (isValidNumber($(control).val()) == false) {
            if ($('span[id=' + validatorname + ']').length == 0) {
                $(control).after("<span style='color:red' id=" + validatorname + ">Invalid Number</span>");
            }
            return false;
        }
        else {
            $('span[id=' + validatorname + ']').remove()
            return true;
        }
    }

}

// convert a string to lowercase with 1st letter capitalised
helper.ucFirst = function(s)
{
	var c = s.charAt(0);

	if (parseInt(s.length)==1){
		return c.toUpperCase();
	}
	else
	{
		return c.toUpperCase() + s.slice(1).toLowerCase();
	}
}


// A simple routine to strip HTML tags from supplied string
// It's not very clever but it's quite useful.
// Everything between each "<" and the subsequent ">" is ignored
// hence it could get confused with any javascript comparisons
// or comments that contain comparisons.
helper.stripHTMLTags = function(str)
{	var mystr="";
	var chr="";
	var skip=false;
	var skipcancel=false;
	
	for (x=0; x<str.length; x++)
	{
		if (skipcancel==true){skip=false;}
		chr=str.charAt(x);
		if (chr=="<"){skip=true;skipcancel=false;}
		else if (chr==">" && skip==true){skipcancel=true;}
		
		if (skip==false) mystr=mystr+chr;
	}
	return mystr;
}


// validate an email address
helper.isValidEmail = function(e)
{
	// assume an email address cannot start with an @ or white space, but it
	// must contain the @ character followed by groups of alphanumerics and '-'
	// followed by the dot character '.'
	// It must end with 2 or 3 alphanumerics.
	//
	var alnum="a-zA-Z0-9";
	exp="^[^@\\s]+@(["+alnum+"+\\-]+\\.)+["+alnum+"]["+alnum+"]["+alnum+"]?$";
	emailregexp = new RegExp(exp);

	result = e.match(emailregexp);
	if (result != null)
	{
		return true;
	}
	else
	{
		return false;
	}
}



helper.isValidNumber = function(numval)
{
	if (numval==""){return false;}
	var myRegExp = new RegExp("^[/+|/-]?[0-9]*[/.]?[0-9]*$");
	return myRegExp.test(numval);
}

helper.isValidDate =  function(d,convert) {
	//var strDatestyle = "US"; //United States date style
	var strDatestyle = "EU";  //European date style
	var strDate;
	var strDateArray;
	var strDay;
	var strMonth;
	var strYear;
	var intDay;
	var intMonth;
	var intYear;
	var booFound = false;
	var strSeparatorArray = new Array("-"," ","/",".");
	var intElementNr;
	var err = 0;
	var strMonthArray = new Array("Jan","Feb","Mar","Apr","May","Jun","Jul","Aug","Sep","Oct","Nov","Dec");
	strDate = d;
	if (strDate.length < 1) {
		return false;
	}
	if (strDate.toLowerCase()=="today" || strDate.toLowerCase()=="now"){return true;}

	for (intElementNr = 0; intElementNr < strSeparatorArray.length; intElementNr++) {
		if (strDate.indexOf(strSeparatorArray[intElementNr]) != -1) {
			strDateArray = strDate.split(strSeparatorArray[intElementNr]);
			if (strDateArray.length != 3) 
			{
				err = 1;
				return false;
			}
			else 
			{
				strDay = strDateArray[0];
				strMonth = strDateArray[1];
				strYear = strDateArray[2];
			}
			booFound = true;
		}
	}

	if (booFound == false) {
		if (strDate.length>5) {
			strDay = strDate.substr(0, 2);
			strMonth = strDate.substr(2, 2);
			strYear = strDate.substr(4);
		}
		else
			return false;
	}
	
	// verify year part	2 or 4 digits
	if (strYear.length != 2 && strYear.length != 4) {return false;}
	if (isNaN(strYear)){return false;}
	// US style (swap month and day)
	if (strDatestyle == "US") {
		strTemp = strDay;
		strDay = strMonth;
		strMonth = strTemp;
	}

	// verify 1 or 2 digit integer day
	if (strDay.length<1 || strDay.length>2) {return false;}
	if (isNaN(strDay)){return false;}
	
	// month may be digits of characters, hence following check
	intMonth = parseInt(strMonth, 10);
	if (isNaN(intMonth)) {
		for (i = 0;i<12;i++) {
			if (strMonth.toUpperCase() == strMonthArray[i].toUpperCase()) {
				intMonth = i+1;
				strMonth = strMonthArray[i];
				i = 12;
			}
		}
		if (isNaN(intMonth)) {
			err = 3;
			return false;
		}
	}

	intDay=parseInt(strDay,10);
	intYear = parseInt(strYear, 10);
	
	if (intMonth>12 || intMonth<1) {
		err = 5;
		return false;
	}
	
	// day in month check
	if (intDay < 1 || intDay > 31){return false;}
		
	if ((intMonth == 4 || intMonth == 6 || intMonth == 9 || intMonth == 11) && (intDay > 30)) {
		return false;
	}
	
	if (intMonth == 2) {
		if (LeapYear(intYear)) {
			if (intDay > 29) {return false;}
		}
		else 
		{
			if (intDay > 28) {return false;}
		}
	}
	
	if (!convert)
		return true;
	else
	{
		if (intYear<=99){intYear=intYear+2000;}
		return intDay+"/"+intMonth+"/"+intYear;
	}
}

helper.ConvertToJscriptDate = function(mydate)
{
	if (!isValidDate(mydate)){return "";}
	var vdate=isValidDate(mydate,true);
	var dparts= vdate.split("/");
	var JDate = new Date(dparts[2]+"/"+dparts[1]+"/"+dparts[0]);
	return JDate;
}

// check a composite date/time field
// assume date is everything up to first space
// and time is everything after first space

helper.isValidDateTime = function(strDateTime)
{
	var dt = Trim(strDateTime);
	var intMatch;
	var intDateOnly = false;

	if (strDateTime.toLowerCase()=="today" || strDateTime.toLowerCase()=="now"){return true;}
	
	intMatch=dt.indexOf(":");
	if (intMatch < 0)
	{
		intDateOnly = true;
		intMatch=dt.length;
	}
	else
	{
		intMatch=dt.indexOf(" ");
	}
	if (intMatch < 0) {return false;}
	
	// check date
	if (!isValidDate(dt.substr(0,intMatch))){return false;}
	
	// check time
	if (!intDateOnly) {
		if (!isValidTime(dt.substr(intMatch+1,dt.length-intMatch))){return false;}
	}
	
	return true;
}


helper.LeapYear = function(intYear) {
	if (intYear % 100 == 0) {
		if (intYear % 400 == 0) { return true; }
	}
	else {
		if ((intYear % 4) == 0) { return true; }
	}
	return false;
}

helper.isEarlierOrEqual = function(start,end)
{
	// convert dates to dd/mm/yyyy
	var myStart = isValidDate(start,true);
	var myEnd = isValidDate(end,true);
	if (myStart=="" || myEnd=="") return false;
	
	var startparts= myStart.split("/");
	var endparts=myEnd.split("/");
	
	if (Date.UTC(startparts[2],startparts[1],startparts[0]) <= Date.UTC(endparts[2],endparts[1],endparts[0]))
		return true;
	else
		return false;
}

helper.isTimeEarlierOrEqual = function(start,end)
{
	// convert times to UTC dates
	if (start=="" || end=="") return false;
	
	var startparts= start.split(":");
	var endparts=end.split(":");
	
	if (Date.UTC(2000,1,1,startparts[0],startparts[1]) <= Date.UTC(2000,1,1,endparts[0],endparts[1]))
		return true;
	else
		return false;
}

// This is the function that performs form verification.  It will be invoked
// from the onSubmit() event handler.  The handler should return whatever
// value this function returns.
helper.frmValidateCommon = function(f)
{
    var msg;
    var empty_fields = "";
    var errors = "";
    var mylen = parseInt(f.length);
    var errOne = false;

    // Loop through the elements of the form, looking for all 
    // text and textarea elements that don't have an "optional" property
    // defined.  Then, check for fields that are empty and make a list of them.
    // Also, if any of these elements have a "min" or a "max" property defined,
    // then verify that they are numbers and that they are in the right range.
    // Put together error messages for fields that are wrong.
    for(var i = 0; i < mylen; i++) {
        var e = f.elements[i];
      
        if (((e.type == "text") || (e.type == "textarea")) && !e.optional) 
        {
            // first check if the field is empty
            if ((e.value == null) || (e.value == "") || isblank(e.value)) 
            {
                empty_fields += "\n          " + ReadableName(e.name);
                if (!errOne)
                {
					errOne=true;e.focus();
				}
                continue;
            }
        }
            
	    // now validate email addresses
	    if (e.email) {
			if ((e.value == null) || (e.value == "") || isblank(e.value)) continue;
			if (!isValidEmail(e.value)) {
				errors += "\n- The value in field " +ReadableName(e.name)+" does not appear to be a valid email address\n"
				if (!errOne){errOne=true;e.focus();}
			}
		}
			
	    // now validate date fields
	    if (e.date) {
			if ((e.value == null) || (e.value == "") || isblank(e.value)) continue;
			if (!isValidDate(e.value)) {
				errors += "\n- The field " +ReadableName(e.name)+" does not contain a recognised\ndate value (try dd-mmm-yyyy) \n"
				if (!errOne){errOne=true;e.focus();}
			}
		}
		
	    // now validate time fields
	    if (e.time) {
			if ((e.value == null) || (e.value == "") || isblank(e.value)) continue;
			if (!isValidTime(e.value)) {
				errors += "\n- The field " +ReadableName(e.name)+" does not contain a recognised\ntime value (try hh:mm[:ss]) \n"
				if (!errOne){errOne=true;e.focus();}
			}
		}
		
	    // now validate date/time fields
	    if (e.datetime) {
			if ((e.value == null) || (e.value == "") || isblank(e.value)) continue;
			if (!isValidDateTime(e.value)) {
				errors += "\n- The field "+ReadableName(e.name)+ " does not contain a recognised\ndate/time value (try dd-mmm-yyyy hh:mm[:ss]) \n"
				if (!errOne){errOne=true;e.focus();}
			}
		}
		
	    // now validate interval fields
	    if (e.interval) {
			if ((e.value == null) || (e.value == "") || isblank(e.value)) continue;
			if (!isValidInterval(e.value)) {
				errors += "\n- The field "+ReadableName(e.name)+ " does not contain a recognised\ninterval value (try nn hours or nn days) \n"
				if (!errOne){errOne=true;e.focus();}
			}
		}
		
        // Now check for fields that are supposed to be numeric.
        if (e.numeric || (e.min != null) || (e.max != null)) 
        { 
			if ((e.value == null) || (e.value == "") || isblank(e.value)) continue;
			var v = parseFloat(e.value);
			if (isNaN(v) || !isValidNumber(e.value) || ((e.min != null) && (v < e.min)) || ((e.max != null) && (v > e.max))) 
			{
					if (!errOne){errOne=true;e.focus();}
					errors += "\n- The field " + ucFirst(e.name) + " must be a number";
                    if (e.min != null) 
                        errors += " that is greater than " + e.min;
                    if (e.max != null && e.min != null) 
                        errors += " and less than " + e.max;
                    else if (e.max != null)
                        errors += " that is less than " + e.max;
                    errors += ".\n";
            }
        }
    }

    // Now, if there were any errors, then display the messages, and
    // return true to prevent the form from being submitted.  Otherwise
    // return false
    if (!empty_fields && !errors) return true;

    msg  = "Sorry, but we cannot process your request because of the\n";
    msg += "following error(s). Please correct the problem and try again\n";

    if (empty_fields) {
        msg += "\n- The following fields are mandatory:" 
                + empty_fields + "\n";
        if (errors) msg += "\n";
    }
    msg += errors;
    alert(msg);
    return false;
}

// this function sets the fields in the combo that exist in the values string
// multiple values should be comma delimited
// e.g. SetSelection(myCombo,"fred,harry");
//    will highlight the combo entries that have the value harry and/or fred
helper.SetSelections = function (Combo,Values)
{	
	var lvals = Values.split(",");
	var strval = "";
	
	for (var x=0; x<Combo.length; x++)
	{
		strval=Combo.options[x].value.toLowerCase();

		for (var i=0; i<lvals.length; i++)
				if (strval==Trim(lvals[i].toLowerCase()))
					Combo.options[x].selected=true;
	}
}


// Insert new item into a combo in alhpabetcial order or at specified pos
helper.InsertIntoList = function(Combo, itemText, itemVal, Pos)
{
	var inserted=false;
	var insertpos=-1;
	
	// add blank row
	Combo.length++;
	
	if (Pos==-1)
	{
		// find alpha pos for insert
		for (var x=0; x<Combo.length-1; x++)
		{	if (Combo.options[x].text.toLowerCase()>itemText.toLowerCase())
				{insertpos=x;break;}
		}
	}
	else
		{insertpos=Pos;}
	
	// shift part of array down one then insert new row	
	if (insertpos!=-1)
	{	for (var y=Combo.length-1; y>insertpos; y--)
		{
			Combo.options[y].value=Combo.options[y-1].value;
			Combo.options[y].text=Combo.options[y-1].text;
		}
		Combo.options[insertpos].text=itemText;
		Combo.options[insertpos].value=itemVal;
	}
	else
	// append new row
	{
		insertpos=Combo.length-1;
		Combo.options[insertpos].text=itemText;
		Combo.options[insertpos].value=itemVal;
	}
	
	return insertpos;
}

// Append a new item into a combo (assume no ordering)
helper.AppendToList = function(Combo, itemText, itemVal)
{
	// add blank row
	Combo.length++;
	Combo.options[Combo.length-1].text=itemText;
	Combo.options[Combo.length-1].value=itemVal;
	return true;
}

// Move selected items up or down one pos in combo list
helper.ShiftListSelections = function(Combo, Down)
{
	var prevtext="";
	var prevval=""
	var swappos;
	if (!Down==true) Down=false;
	
	if (Down==true)
	{
		for (var x=Combo.length-1; x>=0; x--)
		{
			if (Combo[x].selected==true)
			{
				if (x<Combo.length-1 && Combo[x+1].selected==false)
				{
					//swap cur entry for previous entry
					prevtext=Combo[x+1].text;
					prevval=Combo[x+1].value;
					Combo[x+1].text=Combo[x].text;
					Combo[x+1].value=Combo[x].value;
					Combo[x+1].selected=true;
					Combo[x].text=prevtext;
					Combo[x].value=prevval;
					Combo[x].selected=false;
				}	
			}
		}
	}
	else
	{
		for (var x=0; x<Combo.length; x++)
		{
			if (Combo[x].selected==true)
			{
				if (x>0 && Combo[x-1].selected==false)
				{
					//swap cur entry for previous entry
					prevtext=Combo[x-1].text;
					prevval=Combo[x-1].value;
					Combo[x-1].text=Combo[x].text;
					Combo[x-1].value=Combo[x].value;
					Combo[x-1].selected=true;
					Combo[x].text=prevtext;
					Combo[x].value=prevval;
					Combo[x].selected=false;
				}	
			}
		}
	}
}

helper.DeleteFromList = function(Combo, Pos)
{
	if (Pos>=0 && Pos <= Combo.length)
	{
		Combo[Pos]=null;
	}
}

// function to do a bulk move from one list to another
// quicker than doing individual inserts/appends etc
helper.MoveSelectedListItems = function(srcCombo, destCombo, doSort, intSort, useVals)
{
	var numItems = 0;
	var curPos = destCombo.options.length;
	var srcLen = srcCombo.options.length;
	
	for (var x=0; x<srcCombo.options.length; x++)
		if (srcCombo.options[x].selected) numItems++;
	
	destCombo.options.length+=numItems;
	for (var x=0; x<srcCombo.options.length; x++)
		if (srcCombo.options[x].selected==true) 
		{
			destCombo.options[curPos].text=srcCombo.options[x].text;
			destCombo.options[curPos].value=srcCombo.options[x].value;
			curPos++;
		}
	if (doSort) SortList(destCombo, intSort, useVals);
	
	/* now remove the selected items from the source listbox */
	/* This can be very slow for big lists, but I'm not sure how else to do it */		
	for (var x=srcLen-1; x>=0; x--)
		if (srcCombo.options[x].selected) srcCombo.options[x]=null;
}

// Compare functions used internally by the SortList routine,
helper.ListCompareNums = function(a,b)
{
	var la = parseInt(a.split("{")[0]);
	var lb = parseInt(b.split("{")[0]);
	if (la < lb) return -1;
	if (la > lb) return 1
	return 0;
}
helper.ListCompareText = function ListCompareText(a,b)
{	var la = a.toLowerCase();
	var lb = b.toLowerCase();
	if (la < lb) return -1;
	if (la > lb) return 1
	return 0;
}

// Quite Fast way of sorting big option lists
helper.SortList = function(Combo, IntSort, UseVals)
{
	var cmbText = new Array(Combo.options.length);
	var cmbItems;
	
	// get copy of	
	for (x=0; x<cmbText.length; x++)
		cmbText[x]=(UseVals)?Combo.options[x].value + "{" + Combo.options[x].text:Combo.options[x].text + "{" + Combo.options[x].value;
	
	if (IntSort)
		cmbText.sort(ListCompareNums);
	else
		cmbText.sort(ListCompareText);
			
	//rebuild main list, but don't redimension it
	for (x=0; x<cmbText.length; x++){	
		cmbItems = cmbText[x].split("{");
		Combo.options[x].text=(UseVals)?cmbItems[1]:cmbItems[0];	
		Combo.options[x].value=(UseVals)?cmbItems[0]:cmbItems[1];
	}
}




