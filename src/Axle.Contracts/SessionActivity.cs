// (c) Lykke Corporation 2019 - All rights reserved. No copying, adaptation, decompiling, distribution or any other form of use permitted.

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
            string userName,
            string accountId)
        {
            this.Type = type;
            this.SessionId = sessionId;
            this.UserName = userName ?? throw new ArgumentNullException(nameof(userName));
            this.AccountId = accountId;
        }

        [Key(0)]
        public SessionActivityType Type { get; }

        [Key(1)]
        public int SessionId { get; }

        [Key(2)]
        public string UserName { get; }

        [Key(3)]
        public string AccountId { get; }
    }
}
