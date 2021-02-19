// Copyright (c) 2019 Lykke Corp.
// See the LICENSE file in the project root for more information.

namespace Axle.Services
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Dto;
    using Microsoft.AspNetCore.SignalR;

    public interface IHubConnectionService
    {
        Task OpenConnection(
            HubCallerContext context,
            string userName,
            string accountId,
            string clientId,
            string accessToken,
            bool isSupportUser,
            bool isConcurrentConnection);

        void CloseConnection(string connectionId);

        bool TryGetSessionId(string connectionId, out int sessionId);

        IEnumerable<int> GetAllConnectedSessions();

        IEnumerable<string> FindBySessionId(int sessionId);

        IEnumerable<string> FindByAccessToken(string accessToken);

        Task TerminateConnections(TerminateConnectionReason reason, params string[] connectionIds);
    }
}
