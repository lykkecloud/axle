// Copyright (c) Lykke Corp.
// See the LICENSE file in the project root for more information.

namespace Axle.Authorization
{
    using System;
    using Axle.Constants;
    using Microsoft.AspNetCore.Authorization;

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false)]
    public class AccountOwnerOrSupportAttribute : AuthorizeAttribute
    {
        public AccountOwnerOrSupportAttribute()
            : base(AuthorizationPolicies.AccountOwnerOrSupport)
        {
        }
    }
}
