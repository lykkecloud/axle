namespace Axle.Hubs
{
    using System;
    using Axle.Persistence;
    using Microsoft.AspNetCore.SignalR;
    using Serilog;

    public class SessionHubMethods<THub>
        where THub : SessionHub
    {
        private readonly IHubContext<THub> hubContext;
        private readonly ISessionRepository sessionRepository;
        private readonly IReadOnlyRepository<string, HubConnectionContext> connectionRepository;

        public SessionHubMethods(IHubContext<THub> hubContext, ISessionRepository sessionRepository, IReadOnlyRepository<string, HubConnectionContext> connectionRepository)
        {
            this.hubContext = hubContext ?? throw new ArgumentNullException(nameof(hubContext));
            this.sessionRepository = sessionRepository ?? throw new ArgumentNullException(nameof(sessionRepository));
            this.connectionRepository = connectionRepository ?? throw new ArgumentNullException(nameof(connectionRepository));
        }

        public void TerminateSession(string connectionId)
        {
            this.AbortConnection(connectionId);

            var userId = this.sessionRepository.GetSession(connectionId);
            this.sessionRepository.RemoveSession(connectionId);

            Log.Information($"Session {connectionId} terminated by user {userId}.");
        }

        public void StartSession(string connectionId, string userId)
        {
            var activeSessionIds = this.sessionRepository.GetSessionsByUser(userId);
            foreach (var activeSessionId in activeSessionIds)
            {
                this.AbortConnection(activeSessionId);
                this.sessionRepository.RemoveSession(activeSessionId);
            }

            this.sessionRepository.AddSession(connectionId, userId);

            Log.Information($"Session {connectionId} started by user {userId}.");
        }

        private void AbortConnection(string connectionId)
        {
            var connection = this.connectionRepository.Get(connectionId);
            connection?.Abort();
        }
    }
}
