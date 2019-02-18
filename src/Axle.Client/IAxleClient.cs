// Copyright (c) Lykke Corp.
// See the LICENSE file in the project root for more information.

namespace Axle.Client
{
    using Axle.Dto;
    using JetBrains.Annotations;
    using Refit;
    using System.Threading.Tasks;

    [PublicAPI]
    [Headers("Authorization: Bearer")]
    public interface IAxleClient
    {
        [Get("/api/sessions/{userId}")]
        Task<UserSessionResponse> GetUserSession([AliasAs("userId")]string userId);
    }
}
