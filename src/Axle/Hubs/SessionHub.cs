// Copyright (c) Lykke Corp.
// See the LICENSE file in the project root for more information.

namespace Axle.Hubs
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Axle.Persistence;
    using Axle.Services;
    using IdentityModel;
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

        public override Task OnConnectedAsync()
        {
            var sub = this.Context.User.FindFirst(JwtClaimTypes.Subject).Value;
            var clientId = this.Context.User.FindFirst("client_id").Value;
            var token = this.Context.GetHttpContext().Request.Query["access_token"];

            var state = this.sessionLifecycleService.OpenConnection(this.Context.ConnectionId, sub, clientId, token);

            Log.Information($"New connection established (ID: {this.Context.ConnectionId}).");
            this.connectionRepository.Add(this.Context.ConnectionId, this.Context);

            if (state != null)
            {
                foreach (var connection in state.Connections.ToList())
                {
                    this.connectionRepository.Get(connection).Abort();
                }
            }

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
