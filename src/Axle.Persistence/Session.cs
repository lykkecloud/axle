// Copyright (c) 2019 Lykke Corp.
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
            string userName,
            int sessionId,
            string accountId,
            string accessToken,
            string clientId,
            bool isSupportUser)
        {
            UserName = userName ?? throw new ArgumentNullException(nameof(userName));
            AccountId = accountId;
            AccessToken = accessToken ?? throw new ArgumentNullException(nameof(accessToken));
            ClientId = clientId ?? throw new ArgumentNullException(nameof(clientId));
            SessionId = sessionId;
            IsSupportUser = isSupportUser;
        }

        [Key(0)]
        public string UserName { get; }

        [Key(1)]
        public int SessionId { get; }

        [Key(2)]
        public string AccountId { get; }

        [Key(3)]
        public string AccessToken { get; }

        [Key(4)]
        public string ClientId { get; }

        [Key(5)]
        public bool IsSupportUser { get; }
    }
}
