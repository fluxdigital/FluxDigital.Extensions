using System.Web.UI;
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
            showHelpTextForTemplate = !string.IsNullOrEmpty(helpText);

            //foreach (var path in sauronTemplateRoots.GetItems())
            //{
            //    if (PageTemplateItem.Paths.FullPath.ToLower().Contains(path.Paths.FullPath.ToLower()) && !string.IsNullOrEmpty(helpText))
            //    {
            //        showHelpTextForTemplate = true;
            //    }
            //}

            if (showHelpTextForTemplate)
            {
                //add page editor notification
                var pageEditorNotificationType = new PageEditorNotificationType();
                var notification = new PageEditorNotification($"<div class=\"sauron-page-editor-notification\">{string.Format(intro, templateName)}{helpText}</div>",  (PageEditorNotificationType) ExtendedPageEditorNotificationType.Information);
                arguments.Notifications.Add(notification);
            }
        }
    }
}