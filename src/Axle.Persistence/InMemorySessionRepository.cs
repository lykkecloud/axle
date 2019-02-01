// Copyright (c) Lykke Corp.
// See the LICENSE file in the project root for more information.

namespace Axle.Persistence
{
    using System.Collections.Generic;
    using System.Linq;

    // NOTE (Marta): This is going to be replaced in the future with a repository that persists data.
    public sealed class InMemorySessionRepository : ISessionRepository
    {
        private readonly IDictionary<int, SessionState> sessions = new Dictionary<int, SessionState>();

        public void Add(int sessionId, SessionState sessionState)
        {
            this.sessions.Add(sessionId, sessionState);
        }

        public SessionState Get(int sessionId)
        {
            if (this.sessions.TryGetValue(sessionId, out var entity))
            {
                return entity;
            }

            return null;
        }

        public bool TryGet(int sessionId, out SessionState sessionState)
        {
            return this.sessions.TryGetValue(sessionId, out sessionState);
        }

        public void Remove(int sessionId)
        {
            this.sessions.Remove(sessionId);
        }

        public SessionState GetByUser(string userId)
        {
            return this.sessions.Where(x => x.Value.UserId == userId).Select(kv => kv.Value).FirstOrDefault();
        }

        public SessionState GetByConnection(string connectionId)
        {
            return this.sessions.Where(x => x.Value.Connections.Any(id => id == connectionId))
                                .Select(kv => kv.Value)
                                .FirstOrDefault();
        }
    }
}
