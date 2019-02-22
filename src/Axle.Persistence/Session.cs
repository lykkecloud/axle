// Copyright (c) Lykke Corp.
// See the LICENSE file in the project root for more information.

namespace Axle.Persistence
{
    using System;
    using MessagePack;

    [MessagePackObject]
    public class Session
    {
        [SerializationConstructor]
        public Session(
            string userId,
            int sessionId,
            string accountId,
            string accessToken,
            string clientId)
        {
            this.UserId = userId ?? throw new ArgumentNullException(nameof(userId));
            this.AccountId = accountId ?? throw new ArgumentNullException(nameof(accountId));
            this.AccessToken = accessToken ?? throw new ArgumentNullException(nameof(accessToken));
            this.ClientId = clientId ?? throw new ArgumentNullException(nameof(clientId));
            this.SessionId = sessionId;
        }

        [Key(0)]
        public string UserId { get; }

        [Key(1)]
        public int SessionId { get; }

        [Key(2)]
        public string AccountId { get; }

        [Key(3)]
        public string AccessToken { get; }

        [Key(4)]
        public string ClientId { get; }
    }
}
