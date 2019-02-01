// Copyright (c) Lykke Corp.
// See the LICENSE file in the project root for more information.

namespace Axle.Services
{
    using System;
    using System.Linq;
    using Axle.Persistence;

    public class SessionLifecycleService : ISessionLifecycleService
    {
        private readonly ISessionRepository sessionRepository;
        private readonly ITokenRevocationService tokenRevocationService;
        private readonly object syncObj = new object();

        public SessionLifecycleService(
            ISessionRepository sessionRepository,
            ITokenRevocationService tokenRevocationService)
        {
            this.sessionRepository = sessionRepository;
            this.tokenRevocationService = tokenRevocationService;
        }

        public void CloseConnection(string connectionId)
        {
            lock (this.syncObj)
            {
                var state = this.sessionRepository.GetByConnection(connectionId);

                if (state == null)
                {
                    return;
                }

                state.RemoveConnection(connectionId);

                if (!state.Connections.Any())
                {
                    this.sessionRepository.Remove(state.SessionId);
                }
            }
        }

        public SessionState OpenConnection(string connectionId, string userId, string clientId, string accessToken)
        {
            lock (this.syncObj)
            {
                var userInfo = this.sessionRepository.GetByUser(userId);

                if (userInfo != null && userInfo.AccessToken == accessToken)
                {
                    userInfo.AddConnection(connectionId);
                    return null;
                }

                var rand = new Random();
                var sessionId = 0;

                do
                {
                    sessionId = rand.Next(int.MinValue, int.MaxValue);
                }
                while (this.sessionRepository.Get(sessionId) != null);

                var newState = new SessionState(userId, sessionId, accessToken, userId, connectionId);

                this.sessionRepository.Add(sessionId, newState);

                if (userInfo != null)
                {
                    this.sessionRepository.Remove(userInfo.SessionId);
                    this.tokenRevocationService.RevokeAccessToken(userInfo.AccessToken, userInfo.ClientId);
                }

                return userInfo;
            }
        }
    }
}
