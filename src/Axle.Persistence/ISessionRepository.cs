// Copyright (c) 2019 Lykke Corp.
// See the LICENSE file in the project root for more information.

using System.Threading.Tasks;

namespace Axle.Persistence
{
    using System.Collections.Generic;

    public interface ISessionRepository
    {
        Task Add(Session entity);

        Task Update(Session entity);

#pragma warning disable CA1716 // Identifiers should not match keywords
        Task<Session> Get(int sessionId);

        Task<Session> GetByUser(string userName);

        Task<int?> GetSessionIdByUser(string userName);

        Task<Session> GetByAccount(string accountId);

        Task<int?> GetSessionIdByAccount(string accountId);

        Task Remove(int sessionId, string userName, string accountId);

        Task RefreshSessionTimeouts(IEnumerable<int> sessions);

        Task<IEnumerable<Session>> GetExpiredSessions();
    }
}