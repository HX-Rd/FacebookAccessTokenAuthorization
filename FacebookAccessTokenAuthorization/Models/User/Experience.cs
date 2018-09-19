using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace HXRd.Facebook.AccessTokenAuthorization.Models
{
    public class Experience
    {
        [JsonProperty(PropertyName = "id")]
        public string Id;
        [JsonProperty(PropertyName = "description")]
        public string Description;
        [JsonProperty(PropertyName = "from")]
        public FacebookUser From;
        [JsonProperty(PropertyName = "name")]
        public string Name;
        [JsonProperty(PropertyName = "with")]
        public List<FacebookUser> With;
    }
}
