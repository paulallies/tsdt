
var popupStatus = 0;

//loading popup with jQuery magic!
function loadPopup(control){
	//loads popup only if it is disabled
	if(popupStatus==0){
		$("#backgroundPopup").css({
			"opacity": "0.7"
		});
		$("#backgroundPopup").fadeIn("fast");
		$("#" + control).fadeIn("fast");
		popupStatus = 1;
	}
}

//disabling popup with jQuery magic!
function disablePopup(control){
	//disables popup only if it is enabled
	if(popupStatus==1){
	    $("#backgroundPopup").fadeOut("fast");
		$("#" + control).fadeOut("fast");
		popupStatus = 0;
	}
}

//centering popup
function centerPopup(control){
	//request data for centering
	var windowWidth = document.documentElement.clientWidth;
	var windowHeight = document.documentElement.clientHeight;
	var popupHeight = $("#" + control).height();
	var popupWidth = $("#" + control).width();
	//centering
	$("#" + control).css({
		"position": "absolute",
		"top": windowHeight/2-popupHeight/2,
		"left": windowWidth/2-popupWidth/2
	});
	//only need force for IE6
	
	$("#backgroundPopup").css({
		"height": windowHeight
	});
	
}
