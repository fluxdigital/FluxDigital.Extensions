using FluxDigital.Extensions.Core.Enums;
using FluxDigital.Extensions.Core.Services;
using Sitecore.Diagnostics;
using Sitecore.Pipelines.GetPageEditorNotifications;
using Sitecore.Shell.Applications.ContentEditor;

namespace FluxDigital.Extensions.Core.Pipelines.PageEditorNotifications
{
    public class ShowExperienceEditorHelpNotifications : GetPageEditorNotificationsProcessor
    {
        public PageEditorNotificationsService pageEditorNotificationsService = new PageEditorNotificationsService();

        public override void Process(GetPageEditorNotificationsArgs arguments)
        {
            Assert.ArgumentNotNull(arguments, "arguments");
            if (arguments.ContextItem == null) return;

            var contextItem = arguments.ContextItem;
            var templateName = contextItem.TemplateName;
            var templateId = contextItem.TemplateID;
            var helpText = pageEditorNotificationsService.GetPageEditorNotification(templateId);
            var PageTemplateItem = contextItem.Database.GetItem(contextItem.TemplateID);
            bool showHelpTextForTemplate = false;

            var souron = contextItem.Database.GetItem("{499F31BA-737F-4A07-955C-B4EFCFA83B07}");
            Sitecore.Data.Fields.MultilistField souronTemplateRoots = souron.Fields["Page Template Roots"];

            foreach (var path in souronTemplateRoots.GetItems())
            {
                if(PageTemplateItem.Paths.FullPath.ToLower().Contains(path.Paths.FullPath.ToLower()))
                {
                    showHelpTextForTemplate = true;
                }
            }
            
            //var bullet1 = "Ensure you add an image to the hero if you have one.";
            //var bullet2 = "Remove any other content sections that you don't need.";
            if (showHelpTextForTemplate)
            {
                var intro = $"<b>This page is the {templateName} Template. Please follow the guidance below: </b>";
                //var bullets = $"<ul><li>{bullet1}</li><li>{bullet2}</li></ul>";
                //Enum PageEditorNotificationType(){Error,Information,Warning}
                var pageEditorNotificationType = new PageEditorNotificationType() { };
                var notification = new PageEditorNotification($"{intro}{helpText}",
                    (PageEditorNotificationType) ExtendedPageEditorNotificationType.Information);
                //notification.Options.Add(new PageEditorNotificationOption("testing", "testing"));
                arguments.Notifications.Add(notification);
            }
        }
    }
}
