// Copyright (c) 2019 Lykke Corp.
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
            string userName,
            string accountId)
        {
            Type = type;
            SessionId = sessionId;
            UserName = userName ?? throw new ArgumentNullException(nameof(userName));
            AccountId = accountId;
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
