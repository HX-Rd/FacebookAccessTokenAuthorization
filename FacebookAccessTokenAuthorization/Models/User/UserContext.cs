using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace HXRd.Facebook.AccessTokenAuthorization.Models
{
    public class UserContext
    {
        [JsonProperty(PropertyName = "id")]
        public string Id;
    }
}
