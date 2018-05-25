namespace Axle.Persistence
{
    using System.Collections.Generic;

    public interface ISessionRepository : IRepository<string, SessionState>
    {
        bool TryGet(string sessionId, out SessionState sessionState);

        IEnumerable<SessionState> GetByUser(string userId);
    }
}