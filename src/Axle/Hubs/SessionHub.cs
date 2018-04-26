namespace Axle.Hubs
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.SignalR;
    using Serilog;

    public class SessionHub : Hub
    {
        private readonly SessionHubMethods<SessionHub> hubMethods;

        public SessionHub(SessionHubMethods<SessionHub> hubMethods)
        {
            this.hubMethods = hubMethods ?? throw new ArgumentNullException(nameof(hubMethods));
        }

        public static string Name => "session";

        public void TerminateSession()
        {
            this.hubMethods.TerminateSession(this.Context.ConnectionId);
        }

        public void StartSession(string userId)
        {
            this.hubMethods.StartSession(this.Context.Connection, userId);
            Console.WriteLine(this.Context.ConnectionId);
        }

        public override Task OnConnectedAsync()
        {
            Log.Information($"New connection established (ID: {this.Context.ConnectionId}).");
            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            Log.Information($"Disconnected: {this.Context.ConnectionId}).");
            return base.OnDisconnectedAsync(exception);
        }
    }
}
