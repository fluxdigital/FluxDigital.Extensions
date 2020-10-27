//pipeline to inject the ribbon assets in via js
define(["sitecore", "/-/speak/v1/ExperienceEditor/ExperienceEditor.js"], function (Sitecore, ExperienceEditor) {
    return {
        priority: 1,
        execute: function () {
            ExperienceEditor.Common.registerDocumentStyles(["/sitecore modules/Sauron/styles/sauron-page-editor.css"], window.document);
            $(".sauron-page-editor-notification").closest(".alert-info").addClass("sauron-alert-info");
        }
    };
});
