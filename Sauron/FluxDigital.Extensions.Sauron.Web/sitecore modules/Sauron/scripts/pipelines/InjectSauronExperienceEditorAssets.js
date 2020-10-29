//pipeline to inject the ribbon assets in via js
//http://sitecoresuperman.com/a-different-approach-injecting-resources-into-the-experience-editor
define(["sitecore", "/-/speak/v1/ExperienceEditor/ExperienceEditor.js"], function (Sitecore, ExperienceEditor) {
    return {
        priority: 1,
        execute: function () {
            ExperienceEditor.Common.registerDocumentStyles(["/sitecore modules/Sauron/styles/sauron-page-editor.css"], window.document);
            addSauronClass();
        }
    };
});

//needs to wait until require js has loaded all modules before trying to add the classes
function addSauronClass() {
    if ($.isEmptyObject(require.s.contexts._.registry)) {
        $(".sauron-page-editor-notification").closest(".alert-info").addClass("sauron-alert-info");
    }
    else {
        setTimeout(addSauronClass, 10);
    }
}