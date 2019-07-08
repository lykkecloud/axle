// Copyright (c) 2019 Lykke Corp.
// See the LICENSE file in the project root for more information.

namespace Axle
{
    using System.Collections.Generic;
    using System.Security.Claims;
    using System.Threading.Tasks;
    using Axle.Caches;
    using Axle.Constants;
    using IdentityModel;
    using Microsoft.AspNetCore.Authentication;
    using PermissionsManagement.Client;

    public class ClaimsTransformation : IClaimsTransformation
    {
        private readonly IUserRoleToPermissionsTransformer userRoleToPermissionsTransformer;
        private readonly IAccountsCache accountsCache;

        public ClaimsTransformation(
            IUserRoleToPermissionsTransformer userRoleToPermissionsTransformer,
            IAccountsCache accountsCache)
        {
            this.userRoleToPermissionsTransformer = userRoleToPermissionsTransformer;
            this.accountsCache = accountsCache;
        }

        public async Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
        {
            principal = await this.userRoleToPermissionsTransformer.AddPermissionsClaims(principal);

            var claims = new List<Claim>(principal.Claims);

            if (!principal.HasClaim(claim => claim.Type == JwtClaimTypes.Name))
            {
                claims.Add(new Claim(JwtClaimTypes.Name, principal.FindFirst(JwtClaimTypes.ClientId).Value));
            }

            if (!principal.HasClaim(claim => claim.Type == AuthorizationClaims.Accounts))
            {
                var subject = principal.FindFirst(JwtClaimTypes.Subject)?.Value;

                if (!string.IsNullOrWhiteSpace(subject))
                {
                    var accounts = await this.accountsCache.GetAccountIds(subject);

                    foreach (var account in accounts)
                    {
                        claims.Add(new Claim(AuthorizationClaims.Accounts, account, ClaimValueTypes.String));
                    }
                }
            }

            var identity = new ClaimsIdentity(claims, principal.Identity.AuthenticationType, JwtClaimTypes.Name, JwtClaimTypes.Role);

            return new ClaimsPrincipal(identity);
        }
    }
}
