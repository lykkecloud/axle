// Copyright (c) Lykke Corp.
// See the LICENSE file in the project root for more information.

namespace Axle.Caches
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using MarginTrading.AccountsManagement.Contracts;
    using Microsoft.Extensions.Caching.Memory;

    public sealed class AccountsCache : IAccountsCache
    {
        private readonly IAccountsApi accountsApi;
        private readonly IMemoryCache cache;

        public AccountsCache(IAccountsApi accountsApi, IMemoryCache cache)
        {
            this.accountsApi = accountsApi;

            this.cache = cache;
        }

        public Task<List<string>> GetAccountIds(string clientId)
        {
            return this.cache.GetOrCreateAsync(clientId, async entity =>
            {
                entity.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10);

                var userAccounts = await this.accountsApi.GetByClient(clientId);

                return userAccounts?.Select(a => a.Id).ToList();
            });
        }

        public async Task<string> GetClientId(string accountId)
        {
            if (this.cache.TryGetValue(accountId, out string extractedValue))
            {
                return extractedValue;
            }

            foreach (var account in await this.accountsApi.List())
            {
                await this.cache.GetOrCreateAsync(account.Id, entity => Task.FromResult(account.ClientId));
            }

            return this.cache.Get<string>(accountId);
        }
    }
}
