using FluxDigital.Extensions.Sauron.Core.Enums;
using FluxDigital.Extensions.Sauron.Core.Services;
using Sitecore.Configuration;
using Sitecore.Diagnostics;
using Sitecore.Pipelines.GetPageEditorNotifications;

namespace FluxDigital.Extensions.Sauron.Core.Pipelines.PageEditorNotifications
{
    public class ShowExperienceEditorHelpNotifications : GetPageEditorNotificationsProcessor
    {
        public PageEditorNotificationsService PageEditorNotificationsService = new PageEditorNotificationsService();

        /// <summary>
        /// Creates and displays the page editor notification
        /// </summary>
        /// <param name="arguments"></param>
        public override void Process(GetPageEditorNotificationsArgs arguments)
        {
            Assert.ArgumentNotNull(arguments, "arguments");
            if (arguments.ContextItem == null) return;

            var contextItem = arguments.ContextItem;
            var templateName = contextItem.TemplateName;
            var templateId = contextItem.TemplateID;
            var helpText = PageEditorNotificationsService.GetPageEditorNotification(templateId);
            var intro = Settings.GetSetting("SauronPageInfoIntro");

            if (!string.IsNullOrEmpty(helpText))
            {
                //add page editor notification
                //var pageEditorNotificationType = new PageEditorNotificationType(); //TODO: ideally we would set a custom type here and make the view aware of it
                var notification = new PageEditorNotification($"<div class=\"sauron-page-editor-notification\">{string.Format(intro, templateName)}{helpText}</div>",  (PageEditorNotificationType) ExtendedPageEditorNotificationType.Information);
                arguments.Notifications.Add(notification);
            }
        }
    }
}