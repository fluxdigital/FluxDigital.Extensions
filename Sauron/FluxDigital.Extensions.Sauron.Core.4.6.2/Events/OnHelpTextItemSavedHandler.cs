using System;
using System.Runtime.Caching;
using FluxDigital.Extensions.Sauron.Core.Constants;
using Sitecore.Configuration;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.Events;

namespace FluxDigital.Extensions.Sauron.Core.Events
{
    /// <summary>
    /// Clears the cache of Sauron Help Text items After Saving
    /// </summary>
    public class OnHelpTextItemSavedHandler
    {
        public void OnItemSaved(object sender, System.EventArgs args)
        {
            if (args == null)
            {
                return;
            }

            var sauronPageTemplateId = Settings.GetSetting("SauronPageTemplateId");
            var sauronPageTemplateGuid = new ID(sauronPageTemplateId ?? "{948B6791-82E9-42A8-87A1-2F3F6B55ED93}");

            Item item = Event.ExtractParameter(args, 0) as Item;
            if (item != null && item.TemplateID == sauronPageTemplateGuid) //check this is a Sauron Page
            {
                try
                {
                    Log.Info($"Clearing Sauron Cache: {item.Name} updated", this.GetType());

                    //remove the config items cache key from the cache
                    MemoryCache.Default.Remove(SauronConstants.ConfigItemsCacheKey);
                }
                catch (Exception ex)
                {
                    Log.Error(string.Format("Failed clear Sauron Cache {0}: ", item.ID), ex, this);
                }
            }
        }
    }
}
