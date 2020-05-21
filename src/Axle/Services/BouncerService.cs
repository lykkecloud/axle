// Copyright (c) 2019 Lykke Corp.
// See the LICENSE file in the project root for more information.

using System.Net.Http;

namespace Axle.Services
{
    using System;
    using System.Threading.Tasks;
    using IdentityModel.Client;
    using Microsoft.Extensions.Logging;

    public class BouncerService : ITokenRevocationService
    {
        private readonly IDiscoveryCache discoveryCache;
        private readonly IHttpClientFactory httpClientFactory;
        private readonly ILogger<BouncerService> logger;

        private const string ClientSecret = "no-secret-for-public-client";

        public BouncerService(
            IDiscoveryCache discoveryCache,
            ILogger<BouncerService> logger,
            IHttpClientFactory httpClientFactory)
        {
            this.discoveryCache = discoveryCache;
            this.logger = logger;
            this.httpClientFactory = httpClientFactory;
        }

        public async Task RevokeAccessToken(string accessToken, string clientId)
        {
            logger.LogInformation($"Revoking access token: [{accessToken}]");

            var discoveryResponse = await discoveryCache.GetAsync();
            if (discoveryResponse.IsError)
            {
                logger.LogError($"Unable to get token revocation endpoint from ironclad | Error: {discoveryResponse.Error}", discoveryResponse.Exception);
                throw new Exception(discoveryResponse.Error, discoveryResponse.Exception);
            }

            var httpClient = httpClientFactory.CreateClient();
            var result = await httpClient.RevokeTokenAsync(new TokenRevocationRequest
            {
                Address = discoveryResponse.RevocationEndpoint, 
                ClientId = clientId, 
                ClientSecret = ClientSecret,
                Token = accessToken
            });
            
            if (result.IsError)
            {
                logger.LogError($"An error occurred while revoking token | Error: {result.Error}", result.Exception);
                throw new Exception($"An error occurred while revoking token | Error: {result.Error}", result.Exception);
            }

            logger.LogInformation($"Successfully revoked access token: [{accessToken}]");
        }
    }
}
