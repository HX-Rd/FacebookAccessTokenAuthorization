using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace HXRd.Facebook.AccessTokenAuthorization.Models
{
    public class AgeRange
    {
        [JsonProperty(PropertyName = "min")]
        public long? Min;
        [JsonProperty(PropertyName = "max")]
        public long? Max;
    }
}
