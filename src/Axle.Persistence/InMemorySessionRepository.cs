// Copyright (c) Lykke Corp.
// See the LICENSE file in the project root for more information.

namespace Axle.Persistence
{
    using System.Collections.Generic;
    using System.Linq;

    // NOTE (Marta): This is going to be replaced in the future with a repository that persists data.
    public sealed class InMemorySessionRepository : ISessionRepository
    {
        private readonly IDictionary<int, Session> sessions = new Dictionary<int, Session>();

        public void Add(Session session)
        {
            this.sessions.Add(session.SessionId, session);
        }

        public Session Get(int sessionId)
        {
            if (this.sessions.TryGetValue(sessionId, out var entity))
            {
                return entity;
            }

            return null;
        }

        public void Remove(int sessionId, string userId)
        {
            this.sessions.Remove(sessionId);
        }

        public Session GetByUser(string userId)
        {
            return this.sessions.Where(x => x.Value.UserId == userId).Select(kv => kv.Value).FirstOrDefault();
        }

        public void RefreshSessionTimeouts(IEnumerable<Session> sessions)
        {
        }

        public IEnumerable<Session> GetExpiredSessions()
        {
            return Enumerable.Empty<Session>();
        }
    }
}
