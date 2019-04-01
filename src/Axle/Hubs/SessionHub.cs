// (c) Lykke Corporation 2019 - All rights reserved. No copying, adaptation, decompiling, distribution or any other form of use permitted.

namespace Axle.Hubs
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Axle.Constants;
    using Axle.Contracts;
    using Axle.Dto;
    using Axle.Extensions;
    using Axle.Persistence;
    using Axle.Services;
    using IdentityModel;
    using Microsoft.AspNetCore.Authentication.JwtBearer;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.SignalR;
    using Serilog;

    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = AuthorizationPolicies.AccountOwnerOrSupport)]
    public class SessionHub : Hub
    {
        private readonly IRepository<string, HubCallerContext> connectionRepository;
        private readonly ISessionLifecycleService sessionLifecycleService;
        private readonly IHubContext<SessionHub> sessionHubContext;

        public SessionHub(
            IRepository<string, HubCallerContext> connectionRepository,
            ISessionLifecycleService sessionLifecycleService,
            IHubContext<SessionHub> sessionHubContext)
        {
            this.connectionRepository = connectionRepository;
            this.sessionLifecycleService = sessionLifecycleService;
            this.sessionHubContext = sessionHubContext;
            this.sessionLifecycleService.OnCloseConnections += this.TerminateConnections;
        }

        public static string Name => "/session";

        public override async Task OnConnectedAsync()
        {
            var userName = this.Context.User.FindFirst(JwtClaimTypes.Name).Value;
            var clientId = this.Context.User.FindFirst("client_id").Value;

            var query = this.Context.GetHttpContext().Request.Query;

            var token = query["access_token"];
            var accountId = query["account_id"];

            var isSupportUser = this.Context.User.IsSupportUser(accountId);

            await this.sessionLifecycleService.OpenConnection(this.Context.ConnectionId, userName, accountId, clientId, token, isSupportUser);

            Log.Information($"New connection established. User: {userName}, ID: {this.Context.ConnectionId}.");
            this.connectionRepository.Add(this.Context.ConnectionId, this.Context);
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            var sub = this.Context.User.FindFirst(JwtClaimTypes.Subject).Value;

            Log.Information($"Connection closed. User: {sub}, ID: {this.Context.ConnectionId}.");
            this.connectionRepository.Remove(this.Context.ConnectionId);
            this.sessionLifecycleService.CloseConnection(this.Context.ConnectionId);
            return base.OnDisconnectedAsync(exception);
        }

        private void TerminateConnections(IEnumerable<string> connections, SessionActivityType reason)
        {
            foreach (var connection in connections)
            {
                if (reason == SessionActivityType.DifferentDeviceTermination)
                {
                    this.sessionHubContext.Clients.Clients(connection)
                                       .SendAsync("concurrentSessionTermination", StatusCode.IF_ATH_502, StatusCode.IF_ATH_502.ToMessage()).Wait();
                }

                this.connectionRepository.Get(connection).Abort();
            }
        }
    }
}
