namespace Axle.Persistence
{
    using System.Collections.Generic;

    public interface ISessionRepository : IRepository<string, string>
    {
        bool TryGet(string sessionId, out string userId);

        IEnumerable<string> GetByUser(string userId);
    }
}