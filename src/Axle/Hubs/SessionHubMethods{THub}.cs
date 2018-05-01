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
        private readonly IRepository<string, HubConnectionContext> connectionRepository;

        public SessionHubMethods(IHubContext<THub> hubContext, ISessionRepository sessionRepository, IRepository<string, HubConnectionContext> connectionRepository)
        {
            this.hubContext = hubContext ?? throw new ArgumentNullException(nameof(hubContext));
            this.sessionRepository = sessionRepository ?? throw new ArgumentNullException(nameof(sessionRepository));
            this.connectionRepository = connectionRepository ?? throw new ArgumentNullException(nameof(connectionRepository));
        }

        public void TerminateSession(string connectionId)
        {
            var connection = this.connectionRepository.Get(connectionId);

            if (connection != null)
            {
                connection.Abort();

                var userId = this.sessionRepository.GetSession(connectionId);
                this.sessionRepository.RemoveSession(connectionId);

                Log.Information($"Session {connectionId} terminated by user {userId}.");
            }
        }

        public void StartSession(HubConnectionContext connection, string userId)
        {
            var activeSessionIds = this.sessionRepository.GetSessionsByUser(userId);
            foreach (var activeSessionId in activeSessionIds)
            {
                this.TerminateSession(activeSessionId);
            }

            // TODO (Marta): Thread-safety, du-uh!
            this.connectionRepository.Add(connection.ConnectionId, connection);
            this.sessionRepository.AddSession(connection.ConnectionId, userId);

            Log.Information($"Session {connection.ConnectionId} started by user {userId}.");
        }
    }
}
