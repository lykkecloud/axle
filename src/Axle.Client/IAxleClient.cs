// (c) Lykke Corporation 2019 - All rights reserved. No copying, adaptation, decompiling, distribution or any other form of use permitted.

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
