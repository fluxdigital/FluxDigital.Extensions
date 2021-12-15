using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace SitecoreApiTestApp.Model
{
    class Authentication
    {
        [JsonProperty("domain")]
        public string Domain { set; get; }

        [JsonProperty("username")]
        public string Username { set; get; }

        [JsonProperty("password")]
        public string Password { set; get; }
    }
}
