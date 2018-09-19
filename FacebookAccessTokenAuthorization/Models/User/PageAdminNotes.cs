using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace HXRd.Facebook.AccessTokenAuthorization.Models
{
    public class PageAdminNotes
    {
        [JsonProperty(PropertyName = "body")]
        public string Body;
        [JsonProperty(PropertyName = "from")]
        public dynamic From;
        [JsonProperty(PropertyName = "id")]
        public string Id;
        [JsonProperty(PropertyName = "user")]
        public FacebookUser User ;
    }
}
