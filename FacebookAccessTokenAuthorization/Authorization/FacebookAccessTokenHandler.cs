using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using HXRd.Facebook.AccessTokenAuthorization.Models;

namespace HXRd.Facebook.AccessTokenAuthorization
{
    public class FacebookAccessTokenHandler : AuthenticationHandler<FacebookAccessTokenOptions>
    {
        private FacebookHttpClient _facebookClient;
        private IOptionsMonitor<FacebookAccessTokenOptions> _optionsMonitor;
        private IAuthenticationTicketCache _cache;

        public FacebookAccessTokenHandler(IOptionsMonitor<FacebookAccessTokenOptions> optionsMonitor, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock, FacebookHttpClient googleClient, IServiceProvider provider) 
            : base(optionsMonitor, logger, encoder, clock)
        {
            _facebookClient = googleClient;
            _optionsMonitor = optionsMonitor;
            _cache = provider.GetService<IAuthenticationTicketCache>();
        }

        protected async override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            var options = _optionsMonitor.Get(FacebookAccessTokenDefaults.FACEBOOK_TOKEN_SCHEME);
            var headers = Request.Headers;
            string authHeader;
            try
            {
                authHeader = headers.First(h => h.Key == "Authorization").Value.ToString();
            }
            catch (InvalidOperationException)
            {
                return AuthenticateResult.Fail("Authentication header is null");
            }
            var appTokens = options.AppTokens;
            string clientId;
            try
            {
                clientId = headers.First(h => h.Key.ToLower() == "clientid").Value.ToString();
            }
            catch (InvalidOperationException)
            {
                return AuthenticateResult.Fail("ClientId header is null");
            }
            string appToken;
            try
            {
                appToken = appTokens.First(t => t.StartsWith(clientId));
            }
            catch (Exception)
            {
                return AuthenticateResult.Fail("No App Token set for client id");
            }

            var token = authHeader.Substring("Bearer".Length).Trim();
            var cacheTicket = null as AuthenticationTicket;
            if(_cache != null)
                cacheTicket = _cache.GetTicket(token);
            if (cacheTicket != null)
            {
                var exp = long.Parse(cacheTicket.Principal.Claims.FirstOrDefault(c => c.Type == "exp").Value);
                if (exp < DateTimeOffset.Now.ToUnixTimeSeconds())
                {
                    return AuthenticateResult.Fail("Token is expired");
                }
                return AuthenticateResult.Success(cacheTicket);
            }

            var tokenInfo = null as FacebookTokenInfo;
            DateTimeOffset expires;
            var expiresEpoc = null as long?;
            try
            {
                tokenInfo = await _facebookClient.GetTokenInfo(token, appToken);
                long window = Convert.ToInt64(options.ExpieryWindowSeconds);
                expires = DateTimeOffset.FromUnixTimeSeconds(tokenInfo.Data.ExpiresAt - window);
                expiresEpoc = expires.ToUnixTimeSeconds();
                var now = DateTimeOffset.Now.ToUnixTimeSeconds();
                if (expiresEpoc < DateTimeOffset.Now.ToUnixTimeSeconds())
                {
                    return AuthenticateResult.Fail("Token is expired");
                }
                var optionAppId = appToken.Split('|').First();
                if (options.RequireCorrectAppId && tokenInfo.Data.AppId != optionAppId)
                {
                    return AuthenticateResult.Fail("AppId does not match the AppToken");
                }
                if (options.RequiredScopes != null)
                {
                    if (!(tokenInfo.Data.Scopes.Intersect(options.RequiredScopes).Count() == options.RequiredScopes.Count()))
                    {
                        return AuthenticateResult.Fail("Token does not have all required scopes");
                    }
                }
                if (tokenInfo.Data.Error != null)
                {
                    return AuthenticateResult.Fail(tokenInfo.Data.Error.Message);
                }
            }
            catch (TokenInfoException ex)
            {
                return AuthenticateResult.Fail(ex.Message);
            }

            var userInfo = null as FacebookUser;
            try
            {
                userInfo = await _facebookClient.GetMe(token);
            }
            catch (UserInfoException ex)
            {
                return AuthenticateResult.Fail(ex.Message);
            }

            var identity = MapIdentity(tokenInfo, userInfo, this.Scheme.Name);
            var ticket = new AuthenticationTicket(new ClaimsPrincipal(identity), this.Scheme.Name);
            if(_cache != null)
                _cache.SetTicket(token, ticket, expires);
            return AuthenticateResult.Success(ticket);
        }

        private ClaimsIdentity MapIdentity(FacebookTokenInfo tokenInfo, FacebookUser userInfo, string name)
        {
            var claimList = new List<Claim>()
            {
                new Claim("app_id", tokenInfo.Data.AppId),
                new Claim("type", tokenInfo.Data.Type),
                new Claim("sub", tokenInfo.Data.UserId),
                new Claim("user_id", tokenInfo.Data.UserId),
                new Claim(ClaimTypes.NameIdentifier, tokenInfo.Data.UserId),
                new Claim("exp", tokenInfo.Data.ExpiresAt.ToString()),
                new Claim(ClaimTypes.Expiration, tokenInfo.Data.ExpiresAt.ToString()),
                new Claim("expires_at", tokenInfo.Data.ExpiresAt.ToString()),
                new Claim("is_valid", tokenInfo.Data.IsValid.ToString()),
                new Claim("applicaiton", tokenInfo.Data.Application),
                new Claim("scope", String.Join(' ', tokenInfo.Data.Scopes))
            };
            if (userInfo.Id != null) claimList.Add(new Claim("id", userInfo.Id));
            if (userInfo.About != null) claimList.Add(new Claim("about", userInfo.About));
            if (userInfo.Birthday != null) claimList.Add(new Claim("birthday", userInfo.Birthday));
            if (userInfo.Email != null) claimList.Add(new Claim("email", userInfo.Email));
            if (userInfo.Email != null) claimList.Add(new Claim(ClaimTypes.Email, userInfo.Email));
            if (userInfo.FirstName != null) claimList.Add(new Claim("first_name", userInfo.FirstName));
            if (userInfo.InstallType != null) claimList.Add(new Claim("install_type", userInfo.InstallType));
            if (userInfo.Installed != null) claimList.Add(new Claim("Installed", userInfo.Installed.ToString()));
            if (userInfo.IsFamedeeplinkinguser != null) claimList.Add(new Claim("is_famedeeplinkinguser", userInfo.IsFamedeeplinkinguser.ToString()));
            if (userInfo.LastName != null) claimList.Add(new Claim("last_name", userInfo.LastName));
            if (userInfo.MiddleName != null) claimList.Add(new Claim("middle_name", userInfo.MiddleName));
            if (userInfo.Name != null) claimList.Add(new Claim("name", userInfo.Name));
            if (userInfo.Name != null) claimList.Add(new Claim(ClaimTypes.Name, userInfo.Name));
            if (userInfo.NameFormat != null) claimList.Add(new Claim("name_format", userInfo.NameFormat));
            if (userInfo.TestGroup != null) claimList.Add(new Claim("test_group", userInfo.TestGroup.ToString()));
            if (userInfo.ViewerCanSendGift != null) claimList.Add(new Claim("viewer_can_send_gift", userInfo.ViewerCanSendGift.ToString()));
            if (userInfo.CanReviewMeasurementRequest != null) claimList.Add(new Claim("can_review_measurement_request", userInfo.CanReviewMeasurementRequest.ToString()));
            if (userInfo.EmployeeNumber != null) claimList.Add(new Claim("employee_number", userInfo.EmployeeNumber));
            if (userInfo.Gender != null) claimList.Add(new Claim("gender", userInfo.Gender));
            if (userInfo.IsSharedLogin != null) claimList.Add(new Claim("is_shared_login", userInfo.IsSharedLogin.ToString()));
            if (userInfo.Link != null) claimList.Add(new Claim("link", userInfo.Link));
            if (userInfo.Political != null) claimList.Add(new Claim("political", userInfo.Political));
            if (userInfo.ProfilePic != null) claimList.Add(new Claim("profile_pic", userInfo.ProfilePic));
            if (userInfo.PublicKey != null) claimList.Add(new Claim("public_key", userInfo.PublicKey));
            if (userInfo.Quotes != null) claimList.Add(new Claim("quotes", userInfo.Quotes));
            if (userInfo.Religion != null) claimList.Add(new Claim("religion", userInfo.Religion));
            if (userInfo.SharedLoginUpgradeRequiredBy != null) claimList.Add(new Claim("shared_login_upgrade_required_by", userInfo.SharedLoginUpgradeRequiredBy.ToString())); // This has to be better
            if (userInfo.ShortName != null) claimList.Add(new Claim("short_name", userInfo.ShortName));
            if (userInfo.TokenForBusiness != null) claimList.Add(new Claim("token_for_business", userInfo.TokenForBusiness));

            if (userInfo.Address != null)
            {
                if (userInfo.Address.City != null) claimList.Add(new Claim("address.city", userInfo.Address.City));
                if (userInfo.Address.CityId != null) claimList.Add(new Claim("address.city_id", userInfo.Address.CityId.ToString()));
                if (userInfo.Address.Country != null) claimList.Add(new Claim("address.country", userInfo.Address.Country));
                if (userInfo.Address.CountryCode != null) claimList.Add(new Claim("address.country_code", userInfo.Address.CountryCode));
                if (userInfo.Address.Latitude != null) claimList.Add(new Claim("address.latitude", userInfo.Address.Latitude));
                if (userInfo.Address.LocatedIn != null) claimList.Add(new Claim("address.located_in", userInfo.Address.LocatedIn));
                if (userInfo.Address.Longitude != null) claimList.Add(new Claim("address.longitude", userInfo.Address.Longitude.ToString()));
                if (userInfo.Address.Name != null) claimList.Add(new Claim("address.name", userInfo.Address.Name));
                if (userInfo.Address.Region != null) claimList.Add(new Claim("address.region", userInfo.Address.Region));
                if (userInfo.Address.RegionId != null) claimList.Add(new Claim("address.region_id", userInfo.Address.RegionId.ToString()));
                if (userInfo.Address.State != null) claimList.Add(new Claim("address.state", userInfo.Address.State));
                if (userInfo.Address.Street != null) claimList.Add(new Claim("address.street", userInfo.Address.Street));
                if (userInfo.Address.Zip != null) claimList.Add(new Claim("address.zip", userInfo.Address.Zip));
            }

            if (userInfo.AgeRange != null)
            {
                if (userInfo.AgeRange.Min != null) claimList.Add(new Claim("age_range.min", userInfo.AgeRange.Min.ToString()));
                if (userInfo.AgeRange.Max != null) claimList.Add(new Claim("age_range.max", userInfo.AgeRange.Max.ToString()));
            }

            if (userInfo.AdminNotes != null)
            {
                for(int i = 0; i < userInfo.AdminNotes.Count; ++i)
                {
                    // TODO: Dont really want to add the dynamic stuff and the User, maybe later
                    if (userInfo.AdminNotes[i].Body != null) claimList.Add(new Claim($"admin_notes{i + 1}.body", userInfo.AdminNotes[i].Body));
                    if (userInfo.AdminNotes[i].Id != null) claimList.Add(new Claim($"admin_notes{i + 1}.id", userInfo.AdminNotes[i].Id));
                }
            }

            if (userInfo.PaymentPricepoints != null)
            {
                if (userInfo.PaymentPricepoints.Mobile != null)
                {
                    for (int i = 0; i < userInfo.PaymentPricepoints.Mobile.Count; ++i)
                    {
                        if (userInfo.PaymentPricepoints.Mobile[i].Credits != null) claimList.Add(new Claim($"payment_pricepoints.mobile{ i + 1}.credits", userInfo.PaymentPricepoints.Mobile[i].Credits.ToString()));
                        if (userInfo.PaymentPricepoints.Mobile[i].LocalCurrency != null) claimList.Add(new Claim($"payment_pricepoints.mobile{ i + 1}.local_currency", userInfo.PaymentPricepoints.Mobile[i].LocalCurrency));
                        if (userInfo.PaymentPricepoints.Mobile[i].UserPrice != null) claimList.Add(new Claim($"payment_pricepoints.mobile{ i + 1}.user_price", userInfo.PaymentPricepoints.Mobile[i].UserPrice));
                    }
                }
            }

            if (userInfo.SecuritySettings != null)
            {
                if (userInfo.SecuritySettings.SecureBrowsing != null)
                {
                    if (userInfo.SecuritySettings.SecureBrowsing.Enabled != null) claimList.Add(new Claim("security_settings.secure_browsing.enabled", userInfo.SecuritySettings.SecureBrowsing.Enabled.ToString()));
                }
            }

            if (userInfo.VideoUploadLimits != null)
            {
                if (userInfo.VideoUploadLimits.Length != null) claimList.Add(new Claim("video_upload_limits.length", userInfo.VideoUploadLimits.Length.ToString()));
                if (userInfo.VideoUploadLimits.Size != null) claimList.Add(new Claim("video_upload_limits.size", userInfo.VideoUploadLimits.Size.ToString()));
            }

            if (userInfo.Context != null)
            {
                if (userInfo.Context.Id != null) claimList.Add(new Claim("context.id", userInfo.Context.Id));
            }

            if (userInfo.FavoriteAthletes != null)
            {
                for(int i = 0; i < userInfo.FavoriteAthletes.Count; ++i)
                {
                    // TODO Users not here
                    if (userInfo.FavoriteAthletes[i].Description != null) claimList.Add(new Claim($"favorite_athletes{i + 1}.description", userInfo.FavoriteAthletes[i].Description));
                    if (userInfo.FavoriteAthletes[i].Id != null) claimList.Add(new Claim($"favorite_athletes{i + 1}.id", userInfo.FavoriteAthletes[i].Id));
                    if (userInfo.FavoriteAthletes[i].Name != null) claimList.Add(new Claim($"favorite_athletes{i + 1}.name", userInfo.FavoriteAthletes[i].Name));
                }
            }
            if (userInfo.FavoriteTeams != null)
            {
                for(int i = 0; i < userInfo.FavoriteTeams.Count; ++i)
                {
                    // TODO Users not here
                    if (userInfo.FavoriteTeams[i].Description != null) claimList.Add(new Claim($"favorite_teams{i + 1}.description", userInfo.FavoriteTeams[i].Description));
                    if (userInfo.FavoriteTeams[i].Id != null) claimList.Add(new Claim($"favorite_teams{i + 1}.id", userInfo.FavoriteTeams[i].Id));
                    if (userInfo.FavoriteTeams[i].Name != null) claimList.Add(new Claim($"favorite_teams{i + 1}.name", userInfo.FavoriteTeams[i].Name));
                }
            }

            if (userInfo.Labels != null)
            {
                for(int i = 0; i < userInfo.Labels.Count; ++i)
                {
                    if (userInfo.Labels[i].CreationTime != null) claimList.Add(new Claim($"labels{i + 1}.creation_time", userInfo.Labels[i].CreationTime.ToString()));
                    if (userInfo.Labels[i].Id != null) claimList.Add(new Claim($"labels{i + 1}.id", userInfo.Labels[i].Id));
                    if (userInfo.Labels[i].Name != null) claimList.Add(new Claim($"labels{i + 1}.name", userInfo.Labels[i].Name));
                }
            }

            if (userInfo.Languages != null)
            {
                for (int i = 0; i < userInfo.Languages.Count; ++i)
                {
                    // TODO Users not here
                    if (userInfo.Languages[i].Description != null) claimList.Add(new Claim($"languages{i + 1}.description", userInfo.Languages[i].Description));
                    if (userInfo.Languages[i].Id != null) claimList.Add(new Claim($"languages{i + 1}.id", userInfo.Languages[i].Id));
                    if (userInfo.Languages[i].Name != null) claimList.Add(new Claim($"languages{i + 1}.name", userInfo.Languages[i].Name));
                }
            }

            if (userInfo.MeetingFor != null)
            {
                for (int i = 0; i < userInfo.MeetingFor.Count; ++i)
                {
                    if (userInfo.MeetingFor[i] != null) claimList.Add(new Claim($"meeting_for{i + 1}", userInfo.MeetingFor[i]));
                }
            }

            if (userInfo.Sports != null)
            {
                for (int i = 0; i < userInfo.Sports.Count; ++i)
                {
                    // TODO Users not here
                    if (userInfo.Sports[i].Description != null) claimList.Add(new Claim($"sports{i + 1}.description", userInfo.Sports[i].Description));
                    if (userInfo.Sports[i].Id != null) claimList.Add(new Claim($"sports{i + 1}.id", userInfo.Sports[i].Id));
                    if (userInfo.Sports[i].Name != null) claimList.Add(new Claim($"sports{i + 1}.name", userInfo.Sports[i].Name));
                }
            }
            return new ClaimsIdentity(claimList, name);
        }
    }
}
