// (c) Lykke Corporation 2019 - All rights reserved. No copying, adaptation, decompiling, distribution or any other form of use permitted.

namespace Axle.Services
{
    using System.Threading.Tasks;
    using Chest.Client.AutorestClient;

    // TODO: remove this class when sessions are stored by accountId
    public class AccountsService : IAccountsService
    {
        private readonly IChestClient chestClient;

        public AccountsService(IChestClient chestClient)
        {
            this.chestClient = chestClient;
        }

        public async Task<string> GetAccountOwnerUserName(string accountId)
        {
            var account = await this.chestClient.Metadata.GetAsync("metadata", "accounts", accountId);
            return account?.Data["UserId"];
        }
    }
}
