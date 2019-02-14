// Copyright (c) Lykke Corp.
// See the LICENSE file in the project root for more information.

namespace Axle.Services
{
    using System.Threading.Tasks;
    using IAccountsMgmtApi = MarginTrading.AccountsManagement.Contracts.IAccountsApi;

    public class AccountsService : IAccountsService
    {
        private readonly IAccountsMgmtApi accountsMgmtApi;

        public AccountsService(IAccountsMgmtApi accountsMgmtApi)
        {
            this.accountsMgmtApi = accountsMgmtApi;
        }

        public async Task<string> GetAccountOwnerUserId(string accountId)
        {
            var account = await this.accountsMgmtApi.GetById(accountId);

            return account?.ClientId;
        }
    }
}
