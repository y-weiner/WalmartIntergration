using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Mime;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Net.Http.Headers;

namespace WalmartIntergration.Services.WalmartIntergration
{
    public class WalmartHttpHandler : DelegatingHandler
    {
        private readonly string _clientId;
        private readonly string _clientSecret;

        private readonly IConfiguration _configuration;
        public WalmartHttpHandler(IConfiguration configuration)
        {
            _configuration = configuration;
            _clientId = configuration.GetValue<string>("WallmartIntergration:ClientId");
            _clientSecret = configuration.GetValue<string>("WallmartIntergration:ClientSecret");
        }
        protected override async Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            string _clientToken = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{_clientId}:{_clientSecret}"));
            request.Headers.Authorization = new AuthenticationHeaderValue("Basic", _clientToken);
            request.Headers.Add("WM_SVC.NAME", "Walmart Intergration");
            request.Headers.Add("WM_QOS.CORRELATION_ID", Guid.NewGuid().ToString());
            request.Headers.Add(HeaderNames.Accept, MediaTypeNames.Application.Json);
            return await base.SendAsync(request, cancellationToken);
        }
    }
}
