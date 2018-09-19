using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace HXRd.Facebook.AccessTokenAuthorization.Models
{
    public class VideoUploadLimits
    {
        [JsonProperty(PropertyName = "length")]
        public long? Length;
        [JsonProperty(PropertyName = "size")]
        public long? Size;
    }
}
