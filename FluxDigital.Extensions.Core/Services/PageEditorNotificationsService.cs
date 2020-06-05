using System.Linq;
using Sitecore.Data;

namespace FluxDigital.Extensions.Core.Services
{
    public class PageEditorNotificationsService
    {
        private ID basePageConfigId = new ID("{7C384B97-0546-43BB-ADB3-DCC3A90770AF}");
        public string GetPageEditorNotification(ID pageTemplateId)
        {
            var helpText = string.Empty;
            var basePageConfigItem = Sitecore.Context.Database.GetItem(basePageConfigId);
            var pageHelpTextItems = basePageConfigItem?.Axes?.GetDescendants()?.ToList();
            if(pageHelpTextItems!=null)
            {
                var pageHelpTextItem = pageHelpTextItems.Find(i => i.TemplateID.ToString() == "{948B6791-82E9-42A8-87A1-2F3F6B55ED93}" && i.Fields["Template Id"].Value == pageTemplateId.ToString());
                if (pageHelpTextItem != null)
                {
                    helpText = pageHelpTextItem.Fields["Help Text"].Value;
                }
            }
            return helpText;
        }
    }
}
