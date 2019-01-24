// Copyright (c) Lykke Corp.
// See the LICENSE file in the project root for more information.

namespace Axle.Hubs
{
    using System.Collections.Concurrent;
    using System.Linq;
    using Axle.Persistence;
    using Axle.Services;
    using Microsoft.AspNetCore.SignalR;
    using Serilog;

    public class SessionHubMethods<THub>
        where THub : SessionHub
    {
        private readonly ISessionRepository sessionRepository;
        private readonly IReadOnlyRepository<string, HubCallerContext> connectionRepository;
        private readonly ITokenRevocationService tokenRevocationService;
        private readonly ConcurrentDictionary<string, object> locks = new ConcurrentDictionary<string, object>();

        public SessionHubMethods(
            ISessionRepository sessionRepository,
            IReadOnlyRepository<string, HubCallerContext> connectionRepository,
            ITokenRevocationService tokenRevocationService)
        {
            this.sessionRepository = sessionRepository;
            this.connectionRepository = connectionRepository;
            this.tokenRevocationService = tokenRevocationService;
        }

        public void TerminateSession(string sessionId)
        {
            var sessionState = this.sessionRepository.Get(sessionId);
            if (sessionState == null)
            {
                return;
            }

            var lockObject = this.locks.GetOrAdd(sessionState.UserId, new object());
            lock (lockObject)
            {
                foreach (var connection in sessionState.Connections)
                {
                    this.AbortConnection(connection);
                }

                this.sessionRepository.Remove(sessionId);
            }

            Log.Information($"Session {sessionId} terminated by user {sessionState.UserId}.");
        }

        public void StartSession(string connectionId, string userId, string sessionId, string accessToken)
        {
            var lockObject = this.locks.GetOrAdd(userId, new object());

            lock (lockObject)
            {
                this.TerminateOtherSessions(userId, sessionId);

                if (this.sessionRepository.TryGet(sessionId, out var sessionState))
                {
                    sessionState.AddConnection(connectionId);
                }
                else
                {
                    sessionState = new SessionState(userId, sessionId, accessToken, connectionId);
                    this.sessionRepository.Add(sessionId, sessionState);
                }
            }

            Log.Information($"Session {sessionId} started by user {userId}.");
        }

        private void TerminateOtherSessions(string userId, string sessionId)
        {
            var activeSessions = this.sessionRepository.GetByUser(userId).Where(s => s.SessionId != sessionId).ToList();

            foreach (var activeSession in activeSessions)
            {
                foreach (var connection in activeSession.Connections)
                {
                    this.AbortConnection(connection);
                }

                this.tokenRevocationService.RevokeAccessToken(activeSession.AccessToken);

                this.sessionRepository.Remove(activeSession.SessionId);
            }
        }

        private void AbortConnection(string connectionId)
        {
            var connection = this.connectionRepository.Get(connectionId);
            connection?.Abort();
            Log.Information($"Connection {connectionId} aborted.");
        }
    }
}
