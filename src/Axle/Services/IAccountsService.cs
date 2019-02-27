namespace Axle.Services
{
    using System.Threading.Tasks;

    public interface IAccountsService
    {
        Task<string> GetAccountOwnerUserName(string accountId);
    }
}