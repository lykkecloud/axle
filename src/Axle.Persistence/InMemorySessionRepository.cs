namespace Axle.Persistence
{
    using System.Collections.Generic;

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

        public void RemoveSession(string sessionId)
        {
            this.sessions.Remove(sessionId);
        }
    }
}
