// Copyright (c) 2019 Lykke Corp.
// See the LICENSE file in the project root for more information.

namespace Axle.Persistence
{
    using System.Collections.Generic;

    public interface ISessionRepository
    {
        void Add(Session entity);

        void Update(Session entity);

#pragma warning disable CA1716 // Identifiers should not match keywords
        Session Get(int sessionId);

        Session GetByUser(string userName);

        Session GetByAccount(string accountId);

        void Remove(int sessionId, string userName, string accountId);

        void RefreshSessionTimeouts(IEnumerable<int> sessions);

        IEnumerable<Session> GetExpiredSessions();
    }
}