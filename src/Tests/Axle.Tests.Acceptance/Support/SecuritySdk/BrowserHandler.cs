// Copyright (c) Lykke Corp.
// See the LICENSE file in the project root for more information.

namespace Axle.Tests.Acceptance.Support.SecuritySdk
{
    using System;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;

    // thanks to Damian Hickey for this awesome sample
    // https://github.com/damianh/OwinHttpMessageHandler/blob/master/src/OwinHttpMessageHandler/OwinHttpMessageHandler.cs
    public class BrowserHandler : DelegatingHandler
    {
        private readonly CookieContainer cookieContainer = new CookieContainer();

        public BrowserHandler()
            : base(new HttpClientHandler { AllowAutoRedirect = false })
        {
        }

        public bool AllowCookies { get; set; } = true;

        public bool AllowAutoRedirect { get; set; } = true;

        public int ErrorRedirectLimit { get; set; } = 20;

        public int StopRedirectingAfter { get; set; } = int.MaxValue;

        internal Cookie GetCookie(string uri, string name) => this.cookieContainer.GetCookies(new Uri(uri)).Cast<Cookie>().Where(x => x.Name == name).FirstOrDefault();

        internal void RemoveCookie(string uri, string name)
        {
            var cookie = this.GetCookie(uri, name);
            if (cookie != null)
            {
                cookie.Expired = true;
            }
        }

        protected async override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var response = await this.SendCookiesAsync(request, cancellationToken).ConfigureAwait(false);

            int redirectCount = 0;

            while (this.AllowAutoRedirect && ((int)response.StatusCode >= 300 && (int)response.StatusCode < 400) && redirectCount < this.StopRedirectingAfter)
            {
                if (redirectCount >= this.ErrorRedirectLimit)
                {
                    throw new InvalidOperationException($"Too many redirects. Error limit = {redirectCount}");
                }

                var location = response.Headers.Location;
                if (!location.IsAbsoluteUri)
                {
                    location = new Uri(response.RequestMessage.RequestUri, location);
                }

                request = new HttpRequestMessage(HttpMethod.Get, location);

                response = await this.SendCookiesAsync(request, cancellationToken).ConfigureAwait(false);

                redirectCount++;
            }

            return response;
        }

        protected async Task<HttpResponseMessage> SendCookiesAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if (this.AllowCookies)
            {
                var cookieHeader = this.cookieContainer.GetCookieHeader(request.RequestUri);
                if (!string.IsNullOrEmpty(cookieHeader))
                {
                    request.Headers.Add("Cookie", cookieHeader);
                }
            }

            var response = await base.SendAsync(request, cancellationToken).ConfigureAwait(false);

            if (this.AllowCookies && response.Headers.Contains("Set-Cookie"))
            {
                var responseCookieHeader = string.Join(",", response.Headers.GetValues("Set-Cookie"));
                this.cookieContainer.SetCookies(request.RequestUri, responseCookieHeader);
            }

            return response;
        }
    }
}