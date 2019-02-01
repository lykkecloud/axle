// Copyright (c) Lykke Corp.
// See the LICENSE file in the project root for more information.

namespace Axle.Services
{
    using Axle.Persistence;

    public interface ISessionLifecycleService
    {
        SessionState OpenConnection(string connectionId, string userId, string clientId, string accessToken);

        void CloseConnection(string connectionId);
    }
}
