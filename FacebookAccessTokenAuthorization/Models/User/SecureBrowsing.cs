using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace HXRd.Facebook.AccessTokenAuthorization.Models
{
    public class SecureBrowsing
    {
        [JsonProperty(PropertyName = "enabled")]
        public bool? Enabled;
    }
}
