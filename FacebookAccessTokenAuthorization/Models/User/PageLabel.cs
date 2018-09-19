using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace HXRd.Facebook.AccessTokenAuthorization.Models
{
    public class PageLabel
    {
        [JsonProperty(PropertyName = "creation_time")]
        public DateTime CreationTime;
        [JsonProperty(PropertyName = "creator_id")]
        public dynamic CreatorId;
        [JsonProperty(PropertyName = "from")]
        public dynamic From;
        [JsonProperty(PropertyName = "id")]
        public string Id;
        [JsonProperty(PropertyName = "name")]
        public string Name;
    }
}
