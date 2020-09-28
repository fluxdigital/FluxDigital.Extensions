using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using Sitecore;
using Sitecore.Configuration;
using Sitecore.Data;
using Sitecore.Data.Items;
using System.Runtime.Caching;

namespace FluxDigital.Extensions.Core.Services
{
    /// <summary>
    /// Reads all page editor messages from the config item in Sitecore
    /// </summary>
    public class PageEditorNotificationsService
    {
        private readonly ID basePageConfigId = new ID(Settings.GetSetting("SauronBasePageConfigId") ?? "{7C384B97-0546-43BB-ADB3-DCC3A90770AF}");

        public string GetPageEditorNotification(ID pageTemplateId)
        {
            var sauronPageHelpTextFieldName = Settings.GetSetting("SauronPageHelpTextFieldName") ?? "Help Message";
            var helpText = string.Empty;
            var basePageConfigItem = Context.Database.GetItem(basePageConfigId);
            var sauronPageTemplateId = Settings.GetSetting("SauronPageTemplateId");
            var sauronPageTemplateGuid = new ID(sauronPageTemplateId ?? "{948B6791-82E9-42A8-87A1-2F3F6B55ED93}");
            var templateIdFieldName = Settings.GetSetting("SauronPageTemplateIdFieldName") ?? "Template";
            var enabledfieldName = Settings.GetSetting("SauronPageEnableFieldName") ?? "Enable";
            var configItemsCacheKey = "sauron-help-config-items";
            var cacheTimeInMinutes = Settings.GetIntSetting("SauronCacheTimeMins", 5);

            //get enabled help text items
            var sauronCache = MemoryCache.Default;
            var pageHelpMessageItems = (List<Item>)sauronCache[configItemsCacheKey];

            if (pageHelpMessageItems == null)
            {
                //if not in cache then read from Sitecore
                pageHelpMessageItems = basePageConfigItem?.Axes?.GetDescendants()?.Where(i => (i.TemplateID == sauronPageTemplateGuid && int.Parse(i.Fields[enabledfieldName].Value) == 1)).ToList();

                CacheItemPolicy policy = new CacheItemPolicy();
                policy.AbsoluteExpiration = DateTimeOffset.Now.AddMinutes(cacheTimeInMinutes);
                sauronCache.Set(configItemsCacheKey, pageHelpMessageItems, policy);
            }
            var pageHelpTextItem = pageHelpMessageItems?.Find(i => i.Fields[templateIdFieldName].Value == pageTemplateId.ToString());
            if (pageHelpTextItem != null)
            {
                helpText = pageHelpTextItem.Fields[sauronPageHelpTextFieldName].Value;
            }
            
            return helpText;
        }
    }
}