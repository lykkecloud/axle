namespace Axle.Hubs
{
    using System;
    using System.Collections.Concurrent;
    using Axle.Persistence;
    using Microsoft.AspNetCore.SignalR;
    using Serilog;

    public class SessionHubMethods<THub>
        where THub : SessionHub
    {
        private readonly IHubContext<THub> hubContext;
        private readonly ISessionRepository sessionRepository;
        private readonly IReadOnlyRepository<string, HubConnectionContext> connectionRepository;
        private readonly ConcurrentDictionary<string, object> locks = new ConcurrentDictionary<string, object>();

        public SessionHubMethods(IHubContext<THub> hubContext, ISessionRepository sessionRepository, IReadOnlyRepository<string, HubConnectionContext> connectionRepository)
        {
            this.hubContext = hubContext;
            this.sessionRepository = sessionRepository;
            this.connectionRepository = connectionRepository;
        }

        public void TerminateSession(string connectionId)
        {
            this.AbortConnection(connectionId);

            var userId = this.sessionRepository.Get(connectionId);

            var lockObject = this.locks.GetOrAdd(userId, new object());

            lock (lockObject)
            {
                this.sessionRepository.Remove(connectionId);
            }

            Log.Information($"Session {connectionId} terminated by user {userId}.");
        }

        public void StartSession(string connectionId, string userId, string sessionId)
        {
            var lockObject = this.locks.GetOrAdd(userId, new object());

            lock (lockObject)
            {
                var activeSessionIds = this.sessionRepository.GetByUser(userId);
                foreach (var activeSessionId in activeSessionIds)
                {
                    this.AbortConnection(activeSessionId);
                    this.sessionRepository.Remove(activeSessionId);
                }

                this.sessionRepository.Add(connectionId, userId);
            }

            Log.Information($"Session {connectionId} started by user {userId}.");
        }

        private void AbortConnection(string connectionId)
        {
            var connection = this.connectionRepository.Get(connectionId);
            connection?.Abort();
            Log.Information($"Connection {connectionId} aborted.");
        }
    }
}
