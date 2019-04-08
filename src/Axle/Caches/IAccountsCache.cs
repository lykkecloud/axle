// (c) Lykke Corporation 2019 - All rights reserved. No copying, adaptation, decompiling, distribution or any other form of use permitted.

namespace Axle.Caches
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IAccountsCache
    {
        Task<List<string>> GetAccountIds(string clientId);
    }
}
