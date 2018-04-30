// Copyright (c) Lykke Corp.
// See the LICENSE file in the project root for more information.

#pragma warning disable CA2234, CA1054

namespace Axle.Tests.Acceptance.Support.SecuritySdk
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Net.Http;
    using System.Threading.Tasks;
    using FluentAssertions;
    using IdentityModel.Client;

    // HACK (Cameron): This entire class is super brittle and depends heavily upon the IdentityServer rendering - which is not ideal.
    // TODO (Cameron): Refactor this so that it's not so brittle.
    public class BrowserAutomation : HttpClient
    {
        private readonly BrowserHandler handler;
        private readonly string username;
        private readonly string password;

        private HttpResponseMessage loginResult;

        public BrowserAutomation(string username, string password)
            : this(new BrowserHandler(), username, password)
        {
        }

        private BrowserAutomation(BrowserHandler handler, string username, string password)
            : base(handler)
        {
            this.handler = handler;
            this.username = username;
            this.password = password;
        }

        public async Task NavigateToLoginAsync(string url)
        {
            this.loginResult = await this.GetAsync(url).ConfigureAwait(false);
            this.loginResult.StatusCode.Should().Be(HttpStatusCode.OK);
            this.loginResult.RequestMessage.RequestUri.PathAndQuery.Should().NotStartWith("/home/error");
        }

        public Task LoginToAuthorizationServerAsync() => this.Login(false);

        public Task<AuthorizeResponse> LoginToAuthorizationServerAndCaptureRedirectAsync() => this.Login(true);

        private async Task<AuthorizeResponse> Login(bool capture)
        {
            if (this.loginResult == null)
            {
                throw new InvalidOperationException("Cannot login before navigating to login page!");
            }

            // grab the necessary items for the POST...
            var loginPageHtml = await this.loginResult.Content.ReadAsStringAsync().ConfigureAwait(false);
            var actionStartIndex = loginPageHtml.IndexOf("<form method=\"post\" action=\"", 0, StringComparison.OrdinalIgnoreCase) + 28;
            var action = loginPageHtml.Substring(actionStartIndex, loginPageHtml.IndexOf("\"", actionStartIndex, StringComparison.OrdinalIgnoreCase) - actionStartIndex);
            var tokenStartIndex = loginPageHtml.IndexOf("<input name=\"__RequestVerificationToken\" type=\"hidden\" value=\"", 0, StringComparison.OrdinalIgnoreCase) + 62;
            var token = loginPageHtml.Substring(tokenStartIndex, loginPageHtml.IndexOf("\"", tokenStartIndex, StringComparison.OrdinalIgnoreCase) - tokenStartIndex);

            var form = new Dictionary<string, string>
            {
                { "Username", this.username },
                { "Password", this.password },
                { "__RequestVerificationToken", token },
                { "RememberMe", "false" },
            };

            if (capture)
            {
                // here we *do not* want to auto-redirect to the client (2nd redirect)...
                this.handler.StopRedirectingAfter = 1;
            }

            var url = $"{this.loginResult.RequestMessage.RequestUri.Scheme}://{this.loginResult.RequestMessage.RequestUri.Authority}{action}";

            using (var content = new FormUrlEncodedContent(form))
            using (var authorizeResult = await this.PostAsync(url, content).ConfigureAwait(false))
            {
                if (capture)
                {
                    authorizeResult.StatusCode.Should().Be(HttpStatusCode.Found);
                    this.handler.StopRedirectingAfter = 20;
                    return new AuthorizeResponse(authorizeResult.Headers.Location.ToString());
                }

                authorizeResult.StatusCode.Should().Be(HttpStatusCode.OK);

                // NOTE (Cameron): If we've got to this point we're probably dependent upon the browser running some script.
                // We could just read the data out of this result but because I've gone to all the effort to listen on some *magic* port I'm going to finish the job...
                var html = await authorizeResult.Content.ReadAsStringAsync().ConfigureAwait(false);
                await this.ActLikeABrowserWould(html).ConfigureAwait(false);

                return null;
            }
        }

        private async Task ActLikeABrowserWould(string html)
        {
            // grab the necessary items for the POST...
            var urlStartIndex = html.IndexOf("<form method='post' action='", 0, StringComparison.OrdinalIgnoreCase) + 28;
            var url = html.Substring(urlStartIndex, html.IndexOf("'", urlStartIndex, StringComparison.OrdinalIgnoreCase) - urlStartIndex);

            var form = new Dictionary<string, string>();

            var index = 0;

            while (html.Substring(index).Contains("<input type='hidden' name='"))
            {
                var startIndex = html.IndexOf("<input type='hidden' name='", index, StringComparison.OrdinalIgnoreCase) + 27;
                var name = html.Substring(startIndex, html.IndexOf("'", startIndex, StringComparison.OrdinalIgnoreCase) - startIndex);
                var value = html.Substring(startIndex + name.Length + 9, html.IndexOf("'", startIndex + name.Length + 9, StringComparison.OrdinalIgnoreCase) - startIndex - name.Length - 9);

                form.Add(name, value);

                index = startIndex;
            }

            using (var content = new FormUrlEncodedContent(form))
            using (var redirectResult = await this.PostAsync(url, content).ConfigureAwait(false))
            {
                redirectResult.StatusCode.Should().Be(HttpStatusCode.OK);
            }
        }
    }
}