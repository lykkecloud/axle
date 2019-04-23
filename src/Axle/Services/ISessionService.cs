// (c) Lykke Corporation 2019 - All rights reserved. No copying, adaptation, decompiling, distribution or any other form of use permitted.

namespace Axle.Services
{
    using System.Threading.Tasks;
    using Axle.Contracts;
    using Axle.Dto;
    using Axle.Persistence;

    public interface ISessionService
    {
        Task<Session> BeginSession(
            string userName,
            string accountId,
            string clientId,
            string accessToken,
            bool isSupportUser);

        Task<OnBehalfChangeResponse> UpdateOnBehalfState(int sessionId, string onBehalfAccount);

        Task<TerminateSessionResponse> TerminateSession(
            string userName,
            string accountId,
            bool isSupportUser,
            SessionActivityType reason = SessionActivityType.ManualTermination);

        Task TerminateSession(Session userInfo, SessionActivityType reason);

        int GenerateSessionId();
    }
}
