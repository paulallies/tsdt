function mainmenu() {
    $(" #nav ul ").css({ display: "none" }); // Opera Fix
    $(" #nav li").hover(function() {
        $(this).find('ul:first').css({ visibility: "visible", display: "none" }).show();
        $("select").hide();
    }, function() {
        $(this).find('ul:first').css({ visibility: "hidden" });
        $("select").show();
    });
}



$(document).ready(function() {
    mainmenu();
});