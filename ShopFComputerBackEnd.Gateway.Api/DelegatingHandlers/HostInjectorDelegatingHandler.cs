using Microsoft.AspNetCore.Http;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace ShopFComputerBackEnd.Gateway.Api.DelegatingHandlers
{
    public class HostInjectorDelegatingHandler : DelegatingHandler
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public HostInjectorDelegatingHandler(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
        }

        protected override HttpResponseMessage Send(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var host = $"{_httpContextAccessor.HttpContext.Request.Host.Value}";
            request.Headers.Add("Host", host);
            return base.Send(request, cancellationToken);
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var host = $"{_httpContextAccessor.HttpContext.Request.Host.Value}";
            request.Headers.Add("Host", host);
            return base.SendAsync(request, cancellationToken);
        }
    }
}
