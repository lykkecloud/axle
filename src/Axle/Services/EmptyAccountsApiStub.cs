using System.Collections.Generic;
using System.Threading.Tasks;
using MarginTrading.AccountsManagement.Contracts;
using MarginTrading.AccountsManagement.Contracts.Api;
using MarginTrading.AccountsManagement.Contracts.Models;

namespace Axle.Services
{
    public class EmptyAccountsApiStub : IAccountsApi
    {
        public Task<List<AccountContract>> List(string search = null, bool showDeleted = false)
        {
            throw new System.NotImplementedException();
        }

        public Task<PaginatedResponseContract<AccountContract>> ListByPages(string search = null, int? skip = null, int? take = null, bool showDeleted = false)
        {
            throw new System.NotImplementedException();
        }

        public Task<List<AccountContract>> GetByClient(string clientId, bool showDeleted = false)
        {
            return Task.FromResult(new List<AccountContract>());
        }

        public Task<AccountContract> GetByClientAndId(string clientId, string accountId)
        {
            throw new System.NotImplementedException();
        }

        public Task<AccountContract> GetById(string accountId)
        {
            throw new System.NotImplementedException();
        }

        public Task<AccountContract> Create(string clientId, CreateAccountRequestObsolete request)
        {
            throw new System.NotImplementedException();
        }

        public Task<AccountContract> Create(CreateAccountRequest request)
        {
            throw new System.NotImplementedException();
        }

        public Task<AccountContract> Change(string clientId, string accountId, ChangeAccountRequest request)
        {
            throw new System.NotImplementedException();
        }

        public Task<AccountContract> Change(string accountId, ChangeAccountRequest request)
        {
            throw new System.NotImplementedException();
        }

        public Task<string> BeginChargeManually(string accountId, AccountChargeManuallyRequest request)
        {
            throw new System.NotImplementedException();
        }

        public Task<string> BeginDeposit(string accountId, AccountChargeRequest request)
        {
            throw new System.NotImplementedException();
        }

        public Task<string> BeginWithdraw(string accountId, AccountChargeRequest request)
        {
            throw new System.NotImplementedException();
        }

        public Task<WithdrawalResponse> TryBeginWithdraw(string accountId, AccountChargeRequest request)
        {
            throw new System.NotImplementedException();
        }

        public Task<List<AccountContract>> CreateDefaultAccounts(CreateDefaultAccountsRequest request)
        {
            throw new System.NotImplementedException();
        }

        public Task<List<AccountContract>> CreateAccountsForNewBaseAsset(CreateAccountsForBaseAssetRequest request)
        {
            throw new System.NotImplementedException();
        }

        public Task Reset(string accountId)
        {
            throw new System.NotImplementedException();
        }

        public Task<AccountStatContract> GetStat(string accountId)
        {
            throw new System.NotImplementedException();
        }
    }
}