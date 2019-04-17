// (c) Lykke Corporation 2019 - All rights reserved. No copying, adaptation, decompiling, distribution or any other form of use permitted.

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

        void RefreshSessionTimeouts(IEnumerable<Session> sessions);

        IEnumerable<Session> GetExpiredSessions();
    }
}