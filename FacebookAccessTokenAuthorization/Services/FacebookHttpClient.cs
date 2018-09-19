using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using HXRd.Facebook.AccessTokenAuthorization.Models;
using Microsoft.Extensions.Options;

namespace HXRd.Facebook.AccessTokenAuthorization
{
    public class FacebookHttpClient
    {
        private HttpClient _client;
        private IOptionsMonitor<FacebookAccessTokenOptions> _optionsMonitor;
        private ILogger<FacebookHttpClient> _logger;

        public FacebookHttpClient(HttpClient client, IOptionsMonitor<FacebookAccessTokenOptions> optionsMonitor, ILogger<FacebookHttpClient> logger)
        {
            _client = client;
            _optionsMonitor = optionsMonitor;
            _logger = logger;
        }

        public async Task<FacebookTokenInfo> GetTokenInfo(string accessToken, string appToken)
        {
            var url = $"debug_token?input_token={accessToken}&access_token={appToken}";
            var request = new HttpRequestMessage(HttpMethod.Get, url);
            var result = await _client.SendAsync(request);
            if (!result.IsSuccessStatusCode)
            {
                var errorMessage = await result.Content.ReadAsStringAsync();
                throw new TokenInfoException(errorMessage);
            }
            var json = await result.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<FacebookTokenInfo>(json);
        }

        public async Task<FacebookUser> GetMe(string token)
        {
            var options = _optionsMonitor.Get(FacebookAccessTokenDefaults.FACEBOOK_TOKEN_SCHEME);
            var url = $"me?fields={string.Join(',', options.UserFields)}";
            var request = new HttpRequestMessage(HttpMethod.Get, url);
            request.Headers.Add("Authorization", $"Bearer {token}");
            var result = await _client.SendAsync(request);
            if (!result.IsSuccessStatusCode)
            {
                var errorMessage = await result.Content.ReadAsStringAsync();
                throw new UserInfoException(errorMessage);
            }
            var json = await result.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<FacebookUser>(json);
        }
    }
}
