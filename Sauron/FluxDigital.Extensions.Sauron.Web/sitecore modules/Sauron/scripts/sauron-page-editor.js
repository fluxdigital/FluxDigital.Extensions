//add 'sauron-alert-info' class to parent info message div if it contains a 'sauron-page-editor-notification' div. 
//We can then style the notification using this instead of hacking out of the box js files
$(function () {
    $('#scWebEditRibbon').on('load', function () {
        //console.log('scWebEditRibbon iframe loaded successfully');
        //$("#scWebEditRibbon").contents().find(".sauron-page-editor-notification").closest(".alert-info").addClass("sauron-alert-info");
    });
});

