using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace HXRd.Facebook.AccessTokenAuthorization.Models
{
    public class FacebookTokenInfo
    {
        [JsonProperty(PropertyName = "data")]
        public FacebookTokenInfoData Data { get; set; }
    }
    public class FacebookTokenInfoData
    {
        [JsonProperty(PropertyName = "app_id")]
        public string AppId { get; set; }
        [JsonProperty(PropertyName = "type")]
        public string Type { get; set; }
        [JsonProperty(PropertyName = "application")]
        public string Application { get; set; }
        [JsonProperty(PropertyName = "error")]
        public FacebookError Error { get; set; }
        [JsonProperty(PropertyName = "expires_at")]
        public long ExpiresAt { get; set; }
        [JsonProperty(PropertyName = "is_valid")]
        public bool IsValid { get; set; }
        [JsonProperty(PropertyName = "scopes")]
        public List<string> Scopes { get; set; }
        [JsonProperty(PropertyName = "user_id")]
        public string UserId { get; set; }

    }
}
