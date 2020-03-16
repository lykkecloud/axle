// Copyright (c) 2019 Lykke Corp.
// See the LICENSE file in the project root for more information.

namespace Axle.Contracts
{
    using System;
    using System.Threading.Tasks;
    using Axle.Contracts.Models;
    using JetBrains.Annotations;
    using Refit;

    [PublicAPI]
    [Headers("Authorization: Bearer")]
    public interface IAxleClient
    {
        [Get("/api/sessions/{userName}")]
        [Obsolete("Use GET /for-support/{userName} and GET /for-user/{accountId}")]
        Task<UserSessionResponse> GetUserSession([AliasAs("userName")]string userName);

        [Get("/api/sessions/for-support/{userName}")]
        Task<UserSessionResponse> GetSessionForSupport(string userName);

        [Get("/api/sessions/for-user/{accountId}")]
        Task<UserSessionResponse> GetSessionForUser(string accountId);
    }
}
