// Copyright (c) Lykke Corp.
// See the LICENSE file in the project root for more information.

namespace Axle.Persistence
{
    using System.Collections.Generic;
    using System.Linq;

    // NOTE (Marta): This is going to be replaced in the future with a repository that persists data.
    public sealed class InMemorySessionRepository : ISessionRepository
    {
        private readonly IDictionary<string, SessionState> sessions = new Dictionary<string, SessionState>();

        public void Add(string sessionId, SessionState sessionState)
        {
            this.sessions.Add(sessionId, sessionState);
        }

        public SessionState Get(string sessionId)
        {
            if (this.sessions.TryGetValue(sessionId, out var entity))
            {
                return entity;
            }

            return null;
        }

        public bool TryGet(string sessionId, out SessionState sessionState)
        {
            return this.sessions.TryGetValue(sessionId, out sessionState);
        }

        public void Remove(string sessionId)
        {
            this.sessions.Remove(sessionId);
        }

        public IEnumerable<SessionState> GetByUser(string userId)
        {
            return this.sessions.Where(x => x.Value.UserId == userId).Select(kv => kv.Value).ToList();
        }
    }
}
