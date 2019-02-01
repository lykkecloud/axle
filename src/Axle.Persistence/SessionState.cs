// Copyright (c) Lykke Corp.
// See the LICENSE file in the project root for more information.

namespace Axle.Persistence
{
    using System;
    using System.Collections.Generic;

    public class SessionState
    {
        private readonly ISet<string> connectionIds = new HashSet<string>();

        public SessionState(string userId, int sessionId, string accessToken, string clientId, string initialConnection)
        {
            this.UserId = userId ?? throw new ArgumentNullException(nameof(userId));
            this.AccessToken = accessToken ?? throw new ArgumentNullException(nameof(accessToken));
            this.ClientId = clientId ?? throw new ArgumentNullException(nameof(clientId));
            this.SessionId = sessionId;
            this.connectionIds.Add(initialConnection);
        }

        public string UserId { get; }

        public int SessionId { get; }

        public string AccessToken { get; }

        public string ClientId { get; }

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
