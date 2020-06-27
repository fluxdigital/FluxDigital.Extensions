using System.Linq;
using Sitecore;
using Sitecore.Configuration;
using Sitecore.Data;

namespace FluxDigital.Extensions.Core.Services
{
    public class PageEditorNotificationsService
    {
        private readonly ID basePageConfigId = new ID(Settings.GetSetting("SauronBasePageConfigId"));

        public string GetPageEditorNotification(ID pageTemplateId)
        {
            var sauronPageHelpTextFieldName = Settings.GetSetting("SauronPageHelpTextFieldName");
            var helpText = string.Empty;
            var basePageConfigItem = Context.Database.GetItem(basePageConfigId);
            var pageHelpTextItems = basePageConfigItem?.Axes?.GetDescendants()?.ToList();
            if (pageHelpTextItems != null)
            {
                var sauronPageTemplateId = Settings.GetSetting("SauronPageTemplateId");
                var pageHelpTextItem = pageHelpTextItems.Find(i => i.TemplateID.ToString() == sauronPageTemplateId && i.Fields["Template Id"].Value == pageTemplateId.ToString());
                if (pageHelpTextItem != null)
                {
                    helpText = pageHelpTextItem.Fields[sauronPageHelpTextFieldName].Value;
                }
            }

            return helpText;
        }
    }
}