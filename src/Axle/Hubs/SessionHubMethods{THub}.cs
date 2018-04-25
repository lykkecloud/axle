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

        public SessionHubMethods(IHubContext<THub> hubContext, ISessionRepository sessionRepository)
        {
            this.hubContext = hubContext ?? throw new ArgumentNullException(nameof(hubContext));
            this.sessionRepository = sessionRepository ?? throw new ArgumentNullException(nameof(sessionRepository));
        }

        public void TerminateSession(string connectionId)
        {
            this.hubContext.Clients.Client(connectionId).InvokeAsync("terminateSession");

            var userId = this.sessionRepository.GetSession(connectionId);
            this.sessionRepository.RemoveSession(connectionId);

            Log.Information($"Session {connectionId} terminated by user {userId}.");
        }

        public void StartSession(string connectionId, string userId)
        {
            this.sessionRepository.AddSession(connectionId, userId);
            Log.Information($"Session {connectionId} started by user {userId}.");
        }
    }
}
