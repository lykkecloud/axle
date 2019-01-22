// Copyright (c) Lykke Corp.
// See the LICENSE file in the project root for more information.

namespace Axle.Services
{
    using System.Threading.Tasks;

    public interface ITokenRevocationService
    {
        Task RevokeAccessToken(string accessToken);
    }
}