// (c) Lykke Corporation 2019 - All rights reserved. No copying, adaptation, decompiling, distribution or any other form of use permitted.

namespace Axle.Services
{
    using System.Threading.Tasks;
    using Axle.Contracts;
    using Axle.Dto;

    public interface ISessionLifecycleService
    {
        Task OpenConnection(
            string connectionId,
            string userName,
            string accountId,
            string clientId,
            string accessToken,
            bool isSupportUser);

        void CloseConnection(string connectionId);

        Task<OnBehalfChangeResponse> UpdateOnBehalfState(string connectionId, string onBehalfAccount);

        Task<TerminateSessionResponse> TerminateSession(
            string userName,
            string accountId,
            bool isSupportUser,
            SessionActivityType reason = SessionActivityType.ManualTermination);

        int GenerateSessionId();
    }
}
