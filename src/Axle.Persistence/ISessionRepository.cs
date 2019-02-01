// Copyright (c) Lykke Corp.
// See the LICENSE file in the project root for more information.

namespace Axle.Persistence
{
    public interface ISessionRepository : IRepository<int, SessionState>
    {
        bool TryGet(int sessionId, out SessionState sessionState);

        SessionState GetByUser(string userId);

        SessionState GetByConnection(string connectionId);
    }
}