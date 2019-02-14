using System.Threading.Tasks;

namespace Axle.Services
{
    public interface IAccountsService
    {
        Task<string> GetAccountOwnerUserId(string accountId);
    }
}