using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace HXRd.Facebook.AccessTokenAuthorization.Models
{
    public class SecuritySettings
    {
        [JsonProperty(PropertyName = "secure_browsing")]
        public SecureBrowsing SecureBrowsing;
    }
}
