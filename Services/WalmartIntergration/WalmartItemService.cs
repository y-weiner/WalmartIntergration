using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using WalmartIntergration.Models;
using WalmartIntergration.Services.WalmartIntergration;

namespace WalmartIntergration.Services
{
    public class WalmartItemService
    {
        private readonly HttpClient _client;
        private readonly WalmartAccessTokenService _accessTokenService;

        public WalmartItemService(HttpClient client, WalmartAccessTokenService accessTokenService, IConfiguration configuration)
        {
            this._client = client;
            this._accessTokenService = accessTokenService;
            _client.BaseAddress = new Uri(configuration.GetValue<string>("WallmartIntergration:BaseUrl"));
        }

        public async Task<IEnumerable<WalmartItem>> FetchItems()
        {
            var response = await AttemptCall(0);
            response.EnsureSuccessStatusCode();

            var data = JObject.Parse(await response.Content.ReadAsStringAsync());
            return data.GetValue("ItemResponse").ToObject<IEnumerable<WalmartItem>>();
        }

        private async Task<HttpResponseMessage> AttemptCall(int attempts)
        {
            _client.DefaultRequestHeaders.Add("WM_SEC.ACCESS_TOKEN", _accessTokenService.GetAccessToken());
            var response = await _client.GetAsync("v3/items");

            if (attempts < 1 && (response.StatusCode == HttpStatusCode.Unauthorized || response.StatusCode == HttpStatusCode.Forbidden))
            {
                await _accessTokenService.RefreshAccessToken();
                return AttemptCall(++attempts).Result; // post increment, potential infinite recursion otherwise
            }

            return response;
        }
    }
}
