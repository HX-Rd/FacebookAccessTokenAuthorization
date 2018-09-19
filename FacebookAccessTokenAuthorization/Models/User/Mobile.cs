using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace HXRd.Facebook.AccessTokenAuthorization.Models
{
    public class Mobile
    {
        [JsonProperty(PropertyName="credits")]
        public long? Credits;
        [JsonProperty(PropertyName= "local_currency")]
        public string LocalCurrency;
        [JsonProperty(PropertyName= "user_price")]
        public string UserPrice;
    }
}
