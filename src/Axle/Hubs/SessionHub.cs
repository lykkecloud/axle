// Copyright (c) Lykke Corp.
// See the LICENSE file in the project root for more information.

namespace Axle.Hubs
{
    using System;
    using System.Threading.Tasks;
    using Axle.Persistence;
    using Microsoft.AspNetCore.Authentication.JwtBearer;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http.Connections;
    using Microsoft.AspNetCore.SignalR;
    using Serilog;

    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class SessionHub : Hub
    {
        private readonly SessionHubMethods<SessionHub> hubMethods;
        private readonly IRepository<string, HubCallerContext> connectionRepository;

        public SessionHub(SessionHubMethods<SessionHub> hubMethods, IRepository<string, HubCallerContext> connectionRepository)
        {
            this.hubMethods = hubMethods;
            this.connectionRepository = connectionRepository;
        }

        public static string Name => "/session";

        public void TerminateSession()
        {
            this.hubMethods.TerminateSession(this.Context.ConnectionId);
        }

        public void StartSession(string userId, string sessionId)
        {
            var httpContext = this.Context.GetHttpContext();
            var accessToken = httpContext.Request.Query["access_token"];

            this.hubMethods.StartSession(this.Context.ConnectionId, userId, sessionId, accessToken);
        }

        public override Task OnConnectedAsync()
        {
            Log.Information($"New connection established (ID: {this.Context.ConnectionId}).");
            this.connectionRepository.Add(this.Context.ConnectionId,  this.Context);
            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            Log.Information($"Disconnected: {this.Context.ConnectionId}).");
            this.connectionRepository.Remove(this.Context.ConnectionId);
            return base.OnDisconnectedAsync(exception);
        }
    }
}
