// Copyright (c) Lykke Corp.
// See the LICENSE file in the project root for more information.

namespace Axle.Contracts
{
    using System;
    using MessagePack;

    [MessagePackObject]
    public class SessionActivity
    {
        [SerializationConstructor]
        public SessionActivity(
            SessionActivityType type,
            int sessionId,
            string userId,
            string accountId)
        {
            this.Type = type;
            this.SessionId = sessionId;
            this.UserId = userId ?? throw new ArgumentNullException(nameof(userId));
            this.AccountId = accountId ?? throw new ArgumentNullException(nameof(accountId));
        }

        [Key(0)]
        public SessionActivityType Type { get; }

        [Key(1)]
        public int SessionId { get; }

        [Key(2)]
        public string UserId { get; }

        [Key(3)]
        public string AccountId { get; }
    }
}
