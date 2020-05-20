// Copyright (c) 2019 Lykke Corp.
// See the LICENSE file in the project root for more information.

namespace Axle.Services
{
    using System.Threading.Tasks;
    using Contracts;
    using Dto;
    using Persistence;

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

        Task<int> GenerateSessionId();
    }
}
