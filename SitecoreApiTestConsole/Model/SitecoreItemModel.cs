using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace SitecoreApiTestConsole.Model
{
    class SitecoreItemModel
    {
        [JsonProperty("Template Id")]
        public string TemplateId { set; get; }

        [JsonProperty("Item Name")]
        public string ItemName { set; get; }

    }
}
