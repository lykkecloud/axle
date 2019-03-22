// Copyright (c) Lykke Corp.
// See the LICENSE file in the project root for more information.

namespace Axle.Authorization
{
    using System.Threading.Tasks;
    using Axle.Constants;
    using IdentityModel;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Routing;

    public class MobileClientAndAccountOwnerHandler : AuthorizationHandler<MobileClientAndAccountOwnerRequirement>
    {
        private readonly IHttpContextAccessor contextAccessor;

        public MobileClientAndAccountOwnerHandler(IHttpContextAccessor contextAccessor)
        {
            this.contextAccessor = contextAccessor;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, MobileClientAndAccountOwnerRequirement requirement)
        {
            var routeData = this.contextAccessor.HttpContext.GetRouteData();
            var accountId = routeData.Values["accountId"]?.ToString();

            var isMobileClient = context.User.HasClaim(JwtClaimTypes.ClientId, "axle_api:mobile");

            var isAccountOwner = context.User.HasClaim(AuthorizationClaims.Accounts, accountId);

            if (isMobileClient && isAccountOwner)
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}
