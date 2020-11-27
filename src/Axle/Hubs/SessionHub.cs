// Copyright (c) 2019 Lykke Corp.
// See the LICENSE file in the project root for more information.

using System.Security.Claims;
using Axle.Exceptions;

namespace Axle.Hubs
{
    using System;
    using System.Net;
    using System.Threading.Tasks;
    using Constants;
    using Contracts;
    using Dto;
    using Extensions;
    using Services;
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
            var userName = Context.User.FindFirstValue(JwtClaimTypes.Name);
            if (string.IsNullOrEmpty(userName))
            {
                throw new UserClaimIsEmptyException(JwtClaimTypes.Name);
            }

            var clientId = Context.User.FindFirstValue("client_id");
            if (string.IsNullOrEmpty(clientId))
            {
                throw new UserClaimIsEmptyException("client_id");
            }

            var query = Context.GetHttpContext().Request.Query;

            var token = query["access_token"];
            var accountId = query["account_id"];
            if (!bool.TryParse(query["is_concurrent_connection"], out var isConcurrentConnection))
                isConcurrentConnection = false;

            var isSupportUser = Context.User.IsSupportUser(accountId);

            await hubConnectionService.OpenConnection(Context, userName, accountId, clientId, token, isSupportUser, isConcurrentConnection);

            Log.Information($"New connection established. User: {userName}, ID: {Context.ConnectionId}.");
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            var userName = Context.User.FindFirst(JwtClaimTypes.Name).Value;

            Log.Information($"Connection closed. User: {userName}, ID: {Context.ConnectionId}.");
            hubConnectionService.CloseConnection(Context.ConnectionId);

            return base.OnDisconnectedAsync(exception);
        }

        public async Task<bool> SignOut()
        {
            var userName = Context.User.FindFirst(JwtClaimTypes.Name).Value;
            var accountId = Context.GetHttpContext().Request.Query["account_id"];
            var isSupportUser = Context.User.IsSupportUser(accountId);

            var response = await sessionService.TerminateSession(userName, accountId, isSupportUser, SessionActivityType.SignOut);

            return response.Status == TerminateSessionStatus.Terminated;
        }

        public Task<OnBehalfChangeResponse> SetOnBehalfAccount(string accountId)
        {
            ThrowIfUnauthorized(matchAllPermissions: true, Permissions.OnBehalfSelection);

            if (!hubConnectionService.TryGetSessionId(Context.ConnectionId, out int sessionId))
            {
                throw new HubException("The current connection has not been registered");
            }

            return sessionService.UpdateOnBehalfState(sessionId, accountId);
        }

        private void ThrowIfUnauthorized(bool matchAllPermissions = false, params string[] permissions)
        {
            if (!Context.User.IsAuthorized(matchAllPermissions, permissions))
            {
                throw new HubException(
                    $"Action Forbidden ({(int)HttpStatusCode.Forbidden}). " +
                    "The user does not have the required permissions.");
            }
        }
    }
}
