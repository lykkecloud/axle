// Copyright (c) 2019 Lykke Corp.
// See the LICENSE file in the project root for more information.

namespace Axle.Client
{
    using Axle.Client.Models;
    using JetBrains.Annotations;
    using Refit;
    using System.Threading.Tasks;

    [PublicAPI]
    [Headers("Authorization: Bearer")]
    public interface IAxleClient
    {
        [Get("/api/sessions/{userName}")]
        Task<UserSessionResponse> GetUserSession([AliasAs("userName")]string userName);
    }
}
