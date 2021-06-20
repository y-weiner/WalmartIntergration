using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace WalmartIntergration.Services.WalmartIntergration
{
    public class WalmartAccessTokenService
    {
        private static readonly string AccessTokenCacheKey = "accessToken";
        private static readonly string _baseUrl = "https://marketplace.walmartapis.com/";

        private static readonly MemoryCache _cache;

        static WalmartAccessTokenService()
        {
            _cache = new MemoryCache(new MemoryCacheOptions());
        }

        private readonly HttpClient _client;
        public WalmartAccessTokenService(HttpClient client)
        {
            this._client = client;
            _client.BaseAddress = new Uri(_baseUrl);
        }

        public string GetAccessToken()
        {
            if (!_cache.TryGetValue<string>(AccessTokenCacheKey, out string token))
            {
                token = RefreshAccessToken().Result;
            }
            return token;
        }

        public async Task<string> RefreshAccessToken()
        {
            var body = new Dictionary<string, string>
            {
                { "grant_type", "client_credentials" }
            };
            var content = new FormUrlEncodedContent(body);

            var resposnse = await _client.PostAsync("v3/token", content);
            resposnse.EnsureSuccessStatusCode();

            var data = await resposnse.Content.ReadAsStringAsync();
            var tokenData = JObject.Parse(data);
            var tokenType = tokenData.GetValue("token_type").ToObject<string>();
            var token = tokenData.GetValue("access_token").ToObject<string>();
            var expiration = tokenData.GetValue("expires_in").ToObject<int>();
            var cacheOptions = new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromSeconds(expiration));
            _cache.Set(AccessTokenCacheKey, token, cacheOptions);

            return token;
        }
    }
}
