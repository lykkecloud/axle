// Copyright (c) Lykke Corp.
// See the LICENSE file in the project root for more information.

namespace Axle
{
    using System.Collections.Generic;
    using System.Security.Claims;
    using System.Threading.Tasks;
    using IdentityModel;
    using Microsoft.AspNetCore.Authentication;
    using PermissionsManagement.Client;

    public class ClaimsTransformation : IClaimsTransformation
    {
        private readonly IUserRoleToPermissionsTransformer userRoleToPermissionsTransformer;

        public ClaimsTransformation(IUserRoleToPermissionsTransformer userRoleToPermissionsTransformer)
        {
            this.userRoleToPermissionsTransformer = userRoleToPermissionsTransformer;
        }

        public async Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
        {
            principal = await this.userRoleToPermissionsTransformer.AddPermissionsClaims(principal);

            var claims = new List<Claim>(principal.Claims);

            if (!principal.HasClaim(claim => claim.Type == JwtClaimTypes.Name))
            {
                claims.Add(new Claim(JwtClaimTypes.Name, principal.FindFirst(JwtClaimTypes.ClientId).Value));
            }

            var identity = new ClaimsIdentity(claims, principal.Identity.AuthenticationType, JwtClaimTypes.Name, JwtClaimTypes.Role);

            return new ClaimsPrincipal(identity);
        }
    }
}
