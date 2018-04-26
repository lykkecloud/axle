namespace Axle.Persistence
{
    using System.Collections.Generic;

    public interface ISessionRepository
    {
        void AddSession(string sessionId, string userId);

        string GetSession(string sessionId);

        bool TryGetSession(string sessionId, out string userId);

        void RemoveSession(string sessionId);

        IEnumerable<string> GetSessionsByUser(string userId);
    }
}