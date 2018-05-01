namespace Axle.Persistence
{
    using System.Collections.Generic;
    using System.Linq;

    // NOTE (Marta): This is going to be replaced in the future with a repository that persists data.
    public sealed class InMemorySessionRepository : ISessionRepository
    {
        private readonly IDictionary<string, string> sessions = new Dictionary<string, string>();

        public void AddSession(string sessionId, string userId)
        {
            this.sessions.Add(sessionId, userId);
        }

        public string GetSession(string sessionId)
        {
            return this.sessions[sessionId];
        }

        public bool TryGetSession(string sessionId, out string userId)
        {
            return this.sessions.TryGetValue(sessionId, out userId);
        }

        public void RemoveSession(string sessionId)
        {
            this.sessions.Remove(sessionId);
        }

        public IEnumerable<string> GetSessionsByUser(string userId)
        {
            // TODO (Marta): Replace this with an extra dictionary.
            return this.sessions.Where(x => x.Value == userId).Select(kv => kv.Key).ToList();
        }
    }
}
