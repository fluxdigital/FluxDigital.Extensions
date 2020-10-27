using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using FluxDigital.Extensions.Sauron.Core.Constants;
using Sitecore;
using Sitecore.Configuration;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Data.Managers;

namespace FluxDigital.Extensions.Sauron.Core.Services
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
            var cacheTimeInMinutes = Settings.GetIntSetting("SauronCacheTimeMins", 5);

            //get enabled help text items
            var sauronCache = MemoryCache.Default;
            var pageHelpMessageItems = (List<Item>)sauronCache[SauronConstants.ConfigItemsCacheKey];

            if (pageHelpMessageItems == null)
            {
                //if not in cache then read from Sitecore
                //Stopwatch sw = new Stopwatch();
                //sw.Start();
                pageHelpMessageItems = GetHelpItemsRecursive(basePageConfigItem, 1, sauronPageTemplateGuid, enabledfieldName);
                //sw.Stop();
                //Log.Info($"Query to get Sauron page help text items took: {sw.Elapsed.TotalMilliseconds}", this.GetType());
                CacheItemPolicy policy = new CacheItemPolicy();
                policy.AbsoluteExpiration = DateTimeOffset.Now.AddMinutes(cacheTimeInMinutes);
                sauronCache.Set(SauronConstants.ConfigItemsCacheKey, pageHelpMessageItems, policy);
            }
            //get top one matching item
            var pageHelpTextItem = pageHelpMessageItems?.Find(i => i.Fields[templateIdFieldName].Value == pageTemplateId.ToString());
            if (pageHelpTextItem != null)
            {
                helpText = pageHelpTextItem.Fields[sauronPageHelpTextFieldName].Value;
            }
            
            return helpText;
        }

        /// <summary>
        /// This method should be performant as is scoped to a single folder and it's child items
        /// Tests show it takes 120.3908 miliseconds to query all 378 test items when uncached.
        /// TODO: consider adding support here for a content item query
        /// </summary>
        /// <param name="basePageConfigItem"></param>
        /// <param name="sauronPageTemplateGuid"></param>
        /// <param name="enabledfieldName"></param>
        /// <returns></returns>
        public static List<Item> GetHelpItemsRecursive(Item item, int count, ID sauronPageTemplateGuid, string enabledfieldName)
        {
            var helpItems = new List<Item>();
            for (int i = 0; i < count; i++)
            {
                var itemChildren = item.Children;
                var items = itemChildren.Where(itemChild => TemplateManager.GetTemplate(itemChild).ID == sauronPageTemplateGuid && int.Parse(itemChild.Fields[enabledfieldName].Value) == 1);
                helpItems.AddRange(items);
                foreach (Item subItemChild in itemChildren)
                {
                    if (subItemChild.HasChildren)
                    {
                        helpItems.AddRange(GetHelpItemsRecursive(subItemChild, 1, sauronPageTemplateGuid, enabledfieldName));
                    }
                }
            }
            return helpItems;
        }
    }
}