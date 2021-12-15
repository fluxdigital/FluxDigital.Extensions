using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SitecoreApiTestConsole.Model;

namespace SitecoreApiTestApp.Model
{
    class MediaItemModelModel : SitecoreItemModel
    {
        [JsonProperty("Description")]
        public string Description { set; get; }

        [JsonProperty("Alt")]
        public string Alt { set; get; }

        [JsonProperty("File Path")]
        public string FilePath { set; get; }

        [JsonProperty("Blob")]
        public String Blob { set; get; }

        [JsonProperty("Size")]
        public string Size { set; get; }

        [JsonProperty("Mime Type")]
        public string MimeType { set; get; }

        [JsonProperty("Extension")]
        public string Extension { set; get; }

        [JsonProperty("Height")]
        public string Height { set; get; }
        [JsonProperty("Width")]
        public string Width { set; get; }
        [JsonProperty("Dimensions")]
        public string Dimensions { set; get; }
        [JsonProperty("ImageDescription")]
        public string ImageDescription { set; get; }
    }
}
