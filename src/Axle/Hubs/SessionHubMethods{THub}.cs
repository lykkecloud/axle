namespace Axle.Hubs
{
    using System;
    using Axle.Persistence;
    using Microsoft.AspNetCore.SignalR;

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

            var name = this.sessionRepository.GetSession(connectionId);
            this.sessionRepository.RemoveSession(connectionId);
        }

        public void StartSession(string connectionId, string userId)
        {
            this.sessionRepository.AddSession(connectionId, userId);
        }
    }
}
