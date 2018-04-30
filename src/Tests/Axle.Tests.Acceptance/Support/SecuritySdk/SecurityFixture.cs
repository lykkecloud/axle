// Copyright (c) Lykke Corp.
// See the LICENSE file in the project root for more information.

namespace Axle.Tests.Acceptance.Support.SecuritySdk
{
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Threading;
    using System.Threading.Tasks;
    using IdentityModel.OidcClient;
    using Microsoft.Extensions.Configuration;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;
    using Newtonsoft.Json.Serialization;

    public sealed class SecurityFixture
    {
        private readonly IConfigurationRoot configuration;

        public SecurityFixture()
        {
            this.configuration = new ConfigurationBuilder().AddJsonFile("testsettings.json").Build();

            this.Authority = this.configuration.GetValue<string>("authority");
            this.Handler = this.CreateTokenHandler().GetAwaiter().GetResult();
        }

        public string Authority { get; }

        public HttpMessageHandler Handler { get; }

        private static JsonSerializerSettings GetJsonSerializerSettings()
        {
            var settings = new JsonSerializerSettings
            {
                ContractResolver = new DefaultContractResolver { NamingStrategy = new SnakeCaseNamingStrategy() },
                NullValueHandling = NullValueHandling.Ignore,
            };

            settings.Converters.Add(new StringEnumConverter());

            return settings;
        }

        private async Task<HttpMessageHandler> CreateTokenHandler()
        {
            var automation = new BrowserAutomation(this.configuration.GetValue<string>("username"), this.configuration.GetValue<string>("password"));
            var browser = new Browser(automation);
            var options = new OidcClientOptions
            {
                Authority = this.Authority,
                ClientId = this.configuration.GetValue<string>("clientId"),
                RedirectUri = $"http://127.0.0.1:{browser.Port}",
                Scope = this.configuration.GetValue<string>("scope"),
                FilterClaims = false,
                Browser = browser,
            };

            var oidcClient = new OidcClient(options);
            var result = await oidcClient.LoginAsync(new LoginRequest()).ConfigureAwait(false);

            return new TokenHandler(result.AccessToken);
        }

        private sealed class TokenHandler : DelegatingHandler
        {
            private string accessToken;

            public TokenHandler(string accessToken)
                : base(new HttpClientHandler())
            {
                this.accessToken = accessToken;
            }

            protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", this.accessToken);
                return await base.SendAsync(request, cancellationToken).ConfigureAwait(false);
            }
        }
    }
}