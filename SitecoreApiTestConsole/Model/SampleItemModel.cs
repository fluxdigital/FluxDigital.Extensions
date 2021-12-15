using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace SitecoreApiTestConsole.Model
{
    class SampleItemModel : SitecoreItemModel
    {
        [JsonProperty("title")]
        public string Title { set; get; }

        [JsonProperty("text")]
        public string Text { set; get; }
    }
}
