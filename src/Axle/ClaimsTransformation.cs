// Copyright (c) Lykke Corp.
// See the LICENSE file in the project root for more information.

namespace Axle
{
    using System.Collections.Generic;
    using System.Security.Claims;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Authentication;

    public class ClaimsTransformation : IClaimsTransformation
    {
        public Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
        {
            var claims = new List<Claim>(principal.Claims);

            if (!principal.HasClaim(claim => claim.Type == "name"))
            {
                claims.Add(new Claim("name", principal.FindFirst("client_id").Value));
            }

            var identity = new ClaimsIdentity(claims, principal.Identity.AuthenticationType, "name", "role");

            return Task.FromResult(new ClaimsPrincipal(identity));
        }
    }
}
