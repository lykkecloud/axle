// Copyright (c) Lykke Corp.
// See the LICENSE file in the project root for more information.

namespace Axle.Services
{
    using System;
    using System.Threading.Tasks;
    using IdentityModel.Client;
    using Microsoft.Extensions.Logging;

    public class BouncerService : ITokenRevocationService
    {
        private readonly DiscoveryCache discoveryCache;
        private readonly ILogger<BouncerService> logger;

        public BouncerService(
            DiscoveryCache discoveryCache,
            ILogger<BouncerService> logger)
        {
            this.discoveryCache = discoveryCache;
            this.logger = logger;
        }

        public async Task RevokeAccessToken(string accessToken, string clientId)
        {
            this.logger.LogInformation($"Revoking access token: [{accessToken}]");

            var discoveryResponse = await this.discoveryCache.GetAsync();
            if (discoveryResponse.IsError)
            {
                this.logger.LogError($"Unable to get token revocation endpoint from ironclad | Error: {discoveryResponse.Error}", discoveryResponse.Exception);
                throw new Exception(discoveryResponse.Error, discoveryResponse.Exception);
            }

            var clientSecret = "no-secret-for-public-client";

            using (TokenRevocationClient client = new TokenRevocationClient(discoveryResponse.RevocationEndpoint, clientId, clientSecret))
            {
                var result = await client.RevokeAccessTokenAsync(accessToken);

                if (result.IsError)
                {
                    this.logger.LogError($"An error occurred while revoking token | Error: {result.Error}", result.Exception);
                    throw new Exception($"An error occurred while revoking token | Error: {result.Error}", result.Exception);
                }

                this.logger.LogInformation($"Successfully revoked access token: [{accessToken}]");
            }
        }
    }
}
