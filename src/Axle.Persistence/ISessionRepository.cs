// Copyright (c) Lykke Corp.
// See the LICENSE file in the project root for more information.

namespace Axle.Persistence
{
    using System.Collections.Generic;

    public interface ISessionRepository : IRepository<int, Session>
    {
        Session GetByUser(string userId);

        void RefreshSessionTimeouts(IEnumerable<Session> sessions);
    }
}