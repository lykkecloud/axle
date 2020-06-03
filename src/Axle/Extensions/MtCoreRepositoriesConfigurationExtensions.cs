// Copyright (c) 2019 Lykke Corp.
// See the LICENSE file in the project root for more information.

namespace Axle.Extensions
{
    using HttpClients;
    using Lykke.HttpClientGenerator;
    using Lykke.Snow.Common.Startup;
    using Microsoft.Extensions.DependencyInjection;
    using IAccountsMgmtApi = MarginTrading.AccountsManagement.Contracts.IAccountsApi;

    public static class MtCoreRepositoriesConfigurationExtensions
    {
        public static void AddMtCoreDalRepositories(
            this IServiceCollection services,
            string mtCoreAccountManagementEndpoint,
            string mtCoreAccountsApiKey)
        {
            var mtCoreAccountsMgmtClientGenerator = HttpClientGenerator
                .BuildForUrl(mtCoreAccountManagementEndpoint)
                .WithServiceName<MtCoreHttpErrorResponse>("MT Core Account Management Service")
                .WithOptionalApiKey(mtCoreAccountsApiKey)
                .WithoutRetries()
                .Create();

            services.AddSingleton(mtCoreAccountsMgmtClientGenerator.Generate<IAccountsMgmtApi>());
        }

        private static HttpClientGeneratorBuilder WithOptionalApiKey(this HttpClientGeneratorBuilder builder, string apiKey)
        {
            return !string.IsNullOrWhiteSpace(apiKey) ? builder.WithApiKey(apiKey) : builder;
        }
    }
}