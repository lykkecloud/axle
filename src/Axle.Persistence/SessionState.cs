// Copyright (c) Lykke Corp.
// See the LICENSE file in the project root for more information.

namespace Axle.Persistence
{
    using System;
    using System.Collections.Generic;

    public class SessionState
    {
        private readonly ISet<string> connectionIds = new HashSet<string>();

        public SessionState(string userId, string sessionId, string initialConnection)
        {
            this.UserId = userId ?? throw new ArgumentNullException(nameof(userId));
            this.SessionId = sessionId ?? throw new ArgumentNullException(nameof(sessionId));
            this.connectionIds.Add(initialConnection);
        }

        public string UserId { get; }

        public string SessionId { get; }

        public IEnumerable<string> Connections
        {
            get
            {
                return this.connectionIds;
            }
        }

        public void AddConnection(string connectionId)
        {
            this.connectionIds.Add(connectionId);
        }

        public void RemoveConnection(string connectionId)
        {
            this.connectionIds.Remove(connectionId);
        }
    }
}
