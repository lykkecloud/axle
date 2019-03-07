// Copyright (c) Lykke Corp.
// See the LICENSE file in the project root for more information.

namespace Axle.Persistence
{
    using System.Collections.Generic;

    public interface ISessionRepository
    {
        void Add(Session entity);

#pragma warning disable CA1716 // Identifiers should not match keywords
        Session Get(int sessionId);

        Session GetByUser(string userName);

        void Remove(int sessionId, string userName);

        void RefreshSessionTimeouts(IEnumerable<Session> sessions);

        IEnumerable<Session> GetExpiredSessions();
    }
}