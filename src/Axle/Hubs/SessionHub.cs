// Copyright (c) Lykke Corp.
// See the LICENSE file in the project root for more information.

namespace Axle.Hubs
{
    using System;
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Axle.Persistence;
    using Axle.Services;
    using Microsoft.AspNetCore.Authentication.JwtBearer;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http.Connections;
    using Microsoft.AspNetCore.SignalR;
    using Serilog;

    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class SessionHub : Hub
    {
        private readonly IRepository<string, HubCallerContext> connectionRepository;
        private readonly ISessionLifecycleService sessionLifecycleService;

        public SessionHub(
            IRepository<string, HubCallerContext> connectionRepository,
            ISessionLifecycleService sessionLifecycleService)
        {
            this.connectionRepository = connectionRepository;
            this.sessionLifecycleService = sessionLifecycleService;
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
            var clientId = httpContext.User.FindFirst("client_id").Value;

            var state = this.sessionLifecycleService.OpenConnection(this.Context.ConnectionId, userId, token);

            if (state == null)
            {
                return;
            }

            foreach (var connection in state.Connections.ToList())
            {
                this.connectionRepository.Get(connection).Abort();
            }

                this.hubMethods.StartSession(this.Context.ConnectionId, userId, sessionId, accessToken, clientId);
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
            this.sessionLifecycleService.CloseConnection(this.Context.ConnectionId);
            return base.OnDisconnectedAsync(exception);
        }
    }
}
