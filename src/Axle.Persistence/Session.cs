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
            this.UserName = userName ?? throw new ArgumentNullException(nameof(userName));
            this.AccountId = accountId;
            this.AccessToken = accessToken ?? throw new ArgumentNullException(nameof(accessToken));
            this.ClientId = clientId ?? throw new ArgumentNullException(nameof(clientId));
            this.SessionId = sessionId;
            this.IsSupportUser = isSupportUser;
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
