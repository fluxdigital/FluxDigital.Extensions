using FluxDigital.Extensions.Core.Enums;
using FluxDigital.Extensions.Core.Services;
using Sitecore.Configuration;
using Sitecore.Data.Fields;
using Sitecore.Diagnostics;
using Sitecore.Pipelines.GetPageEditorNotifications;

namespace FluxDigital.Extensions.Core.Pipelines.PageEditorNotifications
{
    public class ShowExperienceEditorHelpNotifications : GetPageEditorNotificationsProcessor
    {
        public PageEditorNotificationsService PageEditorNotificationsService = new PageEditorNotificationsService();

        public override void Process(GetPageEditorNotificationsArgs arguments)
        {
            Assert.ArgumentNotNull(arguments, "arguments");
            if (arguments.ContextItem == null) return;

            var contextItem = arguments.ContextItem;
            var templateName = contextItem.TemplateName;
            var templateId = contextItem.TemplateID;
            var helpText = PageEditorNotificationsService.GetPageEditorNotification(templateId);
            var PageTemplateItem = contextItem.Database.GetItem(contextItem.TemplateID);
            var showHelpTextForTemplate = false;
            var intro = Settings.GetSetting("SauronPageInfoIntro");
            var sauronRootItemId = Settings.GetSetting("SauronRootItemId");
            var sauronPageTemplateFieldName = Settings.GetSetting("SauronPageTemplateFieldName");
            var sauronRootItem = contextItem.Database.GetItem(sauronRootItemId);
            MultilistField sauronTemplateRoots = sauronRootItem.Fields[sauronPageTemplateFieldName];

            foreach (var path in sauronTemplateRoots.GetItems())
            {
                if (PageTemplateItem.Paths.FullPath.ToLower().Contains(path.Paths.FullPath.ToLower()))
                {
                    showHelpTextForTemplate = true;
                }
            }

            if (showHelpTextForTemplate)
            {
                var pageEditorNotificationType = new PageEditorNotificationType();
                var notification = new PageEditorNotification($"{string.Format(intro, templateName)}{helpText}",  (PageEditorNotificationType) ExtendedPageEditorNotificationType.Information);
                arguments.Notifications.Add(notification);
            }
        }
    }
}