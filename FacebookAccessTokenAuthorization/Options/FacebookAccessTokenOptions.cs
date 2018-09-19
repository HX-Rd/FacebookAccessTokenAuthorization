using Microsoft.AspNetCore.Authentication;
using System;
using System.Collections.Generic;
using System.Text;

namespace HXRd.Facebook.AccessTokenAuthorization
{
    public class FacebookAccessTokenOptions : AuthenticationSchemeOptions
    {
        public bool UseMemoryCache { get; set; } = false;
        public int ExpieryWindowSeconds { get; set; } = 10;
        public bool RequireCorrectAppId { get; set; } = false;
        public List<string> AppTokens { get; set; }
        public List<string> RequiredScopes { get; set; }
        public List<string> UserFields { get; set; }
    }
}
