// Copyright (c) Lykke Corp.
// See the LICENSE file in the project root for more information.

namespace Axle.Authorization
{
    using System;
    using System.Linq;
    using System.Security.Claims;
    using System.Threading.Tasks;
    using Axle.Constants;
    using Axle.Services;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http;

    public class AccountOwnerOrSupportHandler : AuthorizationHandler<AccountOwnerOrSupportRequirement>
    {
        private readonly IHttpContextAccessor contextAccessor;
        private readonly IAccountsService accountsService;

        public AccountOwnerOrSupportHandler(IHttpContextAccessor contextAccessor, IAccountsService accountsService)
        {
            this.contextAccessor = contextAccessor;
            this.accountsService = accountsService;
        }

        public static bool IsSupportUser(string accountId, ClaimsPrincipal user)
        {
            var accountIdEmpty = string.IsNullOrWhiteSpace(accountId);

            var isOnBehalf = !accountIdEmpty && user.HasClaim(Permissions.OnBehalfSelection, Permissions.OnBehalfSelection);
            var isSupport = accountIdEmpty && user.HasClaim(Permissions.StartSessionWithoutAcc, Permissions.StartSessionWithoutAcc);

            return isOnBehalf || isSupport;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, AccountOwnerOrSupportRequirement requirement)
        {
            var httpContext = this.contextAccessor.HttpContext;

            var accountId = this.contextAccessor.HttpContext.Request.Query["account_id"].ToString();
            var accountIdEmpty = string.IsNullOrWhiteSpace(accountId);

            var isAccountOwner = !accountIdEmpty && this.IsAccountOwner(context, accountId);

            if (isAccountOwner || IsSupportUser(accountId, context.User))
            {
                // Accounts for the case where a support user tries to connect on behalf a nonexistent account
                if (accountIdEmpty || !string.IsNullOrEmpty(await this.accountsService.GetAccountOwnerUserId(accountId)))
                {
                    context.Succeed(requirement);
                }
            }
        }

        private bool IsAccountOwner(AuthorizationHandlerContext context, string accountId)
        {
            if (context.User.FindAll(AuthorizationClaims.Accounts).Any(scope => string.Equals(scope.Value, accountId, StringComparison.OrdinalIgnoreCase)))
            {
                return true;
            }

            return false;
        }
    }
}
