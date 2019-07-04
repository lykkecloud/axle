// Copyright (c) 2019 Lykke Corp.
// See the LICENSE file in the project root for more information.

namespace Axle.Hubs
{
    using System;
    using System.Net;
    using System.Threading.Tasks;
    using Axle.Constants;
    using Axle.Contracts;
    using Axle.Dto;
    using Axle.Extensions;
    using Axle.Services;
    using IdentityModel;
    using Microsoft.AspNetCore.Authentication.JwtBearer;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.SignalR;
    using PermissionsManagement.Client.Extensions;
    using Serilog;

    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = AuthorizationPolicies.AccountOwnerOrSupport)]
    public class SessionHub : Hub
    {
        private readonly IHubConnectionService hubConnectionService;
        private readonly ISessionService sessionService;

        public SessionHub(
            IHubConnectionService hubConnectionService,
            ISessionService sessionService)
        {
            this.hubConnectionService = hubConnectionService;
            this.sessionService = sessionService;
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

            await this.hubConnectionService.OpenConnection(this.Context, userName, accountId, clientId, token, isSupportUser);

            Log.Information($"New connection established. User: {userName}, ID: {this.Context.ConnectionId}.");
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            var userName = this.Context.User.FindFirst(JwtClaimTypes.Name).Value;

            Log.Information($"Connection closed. User: {userName}, ID: {this.Context.ConnectionId}.");
            this.hubConnectionService.CloseConnection(this.Context.ConnectionId);

            return base.OnDisconnectedAsync(exception);
        }

        public async Task<bool> SignOut()
        {
            var userName = this.Context.User.FindFirst(JwtClaimTypes.Name).Value;
            var accountId = this.Context.GetHttpContext().Request.Query["account_id"];
            var isSupportUser = this.Context.User.IsSupportUser(accountId);

            var response = await this.sessionService.TerminateSession(userName, accountId, isSupportUser, SessionActivityType.SignOut);

            return response.Status == TerminateSessionStatus.Terminated;
        }

        public Task<OnBehalfChangeResponse> SetOnBehalfAccount(string accountId)
        {
            this.ThrowIfUnauthorized(matchAllPermissions: true, Permissions.OnBehalfSelection);

            if (!this.hubConnectionService.TryGetSessionId(this.Context.ConnectionId, out int sessionId))
            {
                throw new HubException("The current connection has not been registered");
            }

            return this.sessionService.UpdateOnBehalfState(sessionId, accountId);
        }

        private void ThrowIfUnauthorized(bool matchAllPermissions = false, params string[] permissions)
        {
            if (!this.Context.User.IsAuthorized(matchAllPermissions, permissions))
            {
                throw new HubException(
                    $"Action Forbidden ({(int)HttpStatusCode.Forbidden}). " +
                    "The user does not have the required permissions.");
            }
        }
    }
}
