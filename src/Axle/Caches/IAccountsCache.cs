// Copyright (c) Lykke Corp.
// See the LICENSE file in the project root for more information.

namespace Axle.Caches
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IAccountsCache
    {
        Task<List<string>> GetAccountIds(string clientId);
    }
}
