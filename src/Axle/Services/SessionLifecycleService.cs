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
        private readonly object syncObj = new object();

        public SessionLifecycleService(ISessionRepository sessionRepository)
        {
            this.sessionRepository = sessionRepository;
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

        public SessionState OpenConnection(string connectionId, string userId, string token)
        {
            lock (this.syncObj)
            {
                var userInfo = this.sessionRepository.GetByUser(userId);

                if (userInfo != null && userInfo.Token == token)
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

                this.sessionRepository.Add(sessionId, new SessionState(userId, token, sessionId, connectionId));

                return userInfo;
            }
        }
    }
}
