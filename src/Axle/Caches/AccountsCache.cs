// (c) Lykke Corporation 2019 - All rights reserved. No copying, adaptation, decompiling, distribution or any other form of use permitted.

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
    }
}
