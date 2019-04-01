// (c) Lykke Corporation 2019 - All rights reserved. No copying, adaptation, decompiling, distribution or any other form of use permitted.

namespace Axle.Services
{
    using System.Threading.Tasks;

    public interface ITokenRevocationService
    {
        Task RevokeAccessToken(string accessToken, string clientId);
    }
}