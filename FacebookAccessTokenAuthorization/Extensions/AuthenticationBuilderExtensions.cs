using Microsoft.AspNetCore.Authentication;
using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace HXRd.Facebook.AccessTokenAuthorization
{
    public static class AuthenticationBuilderExtensions
    {
        public static AuthenticationBuilder AddFacebookTokenAuthorization(this AuthenticationBuilder builder)
        {
            return AddFacebookTokenAuthorization(builder, FacebookAccessTokenDefaults.FACEBOOK_TOKEN_SCHEME, null, o => { });
        }
        public static AuthenticationBuilder AddFacebookTokenAuthorization(this AuthenticationBuilder builder, Action<FacebookAccessTokenOptions> configurationOptions)
        {
            return AddFacebookTokenAuthorization(builder, FacebookAccessTokenDefaults.FACEBOOK_TOKEN_SCHEME, null, configurationOptions);
        }

        public static AuthenticationBuilder AddFacebookTokenAuthorization(this AuthenticationBuilder builder, string authenticationScheme)
        {
            return AddFacebookTokenAuthorization(builder, authenticationScheme, null, o => { });
        }

        public static AuthenticationBuilder AddFacebookTokenAuthorization(this AuthenticationBuilder builder, string authenticationScheme, Action<FacebookAccessTokenOptions> configureOptions)
        {
            return AddFacebookTokenAuthorization(builder, authenticationScheme, null, configureOptions);
        }

        public static AuthenticationBuilder AddFacebookTokenAuthorization(this AuthenticationBuilder builder, string authenticationScheme, string displayName, Action<FacebookAccessTokenOptions> configureOptions)
        {
            builder.Services.AddHttpClient<FacebookHttpClient>(client => client.BaseAddress = new Uri("https://graph.facebook.com"));
            var @return = null as AuthenticationBuilder;
            if (displayName == null)
                @return = builder.AddScheme<FacebookAccessTokenOptions, FacebookAccessTokenHandler>(authenticationScheme, configureOptions);
            @return = builder.AddScheme<FacebookAccessTokenOptions, FacebookAccessTokenHandler>(authenticationScheme, displayName, configureOptions);
            var provider = builder.Services.BuildServiceProvider();
            var options = provider.GetService<IOptionsMonitor<FacebookAccessTokenOptions>>().Get(FacebookAccessTokenDefaults.FACEBOOK_TOKEN_SCHEME);
            if (options.UseMemoryCache) 
                builder.Services.AddSingleton<IAuthenticationTicketCache, AuthenticationTicketMemoryCache>();
            return @return;
        }
    }
}
