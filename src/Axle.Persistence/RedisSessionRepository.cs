// (c) Lykke Corporation 2019 - All rights reserved. No copying, adaptation, decompiling, distribution or any other form of use permitted.

namespace Axle.Persistence
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using MessagePack;
    using StackExchange.Redis;

    public class RedisSessionRepository : ISessionRepository
    {
        private readonly IConnectionMultiplexer multiplexer;
        private readonly TimeSpan sessionTimeout;

        public RedisSessionRepository(IConnectionMultiplexer multiplexer, TimeSpan sessionTimeout)
        {
            this.multiplexer = multiplexer;
            this.sessionTimeout = sessionTimeout;
        }

        private static string ExpirationSetKey => "axle:expirations";

        public void Add(Session session)
        {
            var serSession = MessagePackSerializer.Serialize(session);
            var unixNow = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

            var db = this.multiplexer.GetDatabase();

            var transaction = db.CreateTransaction();

            transaction.StringSetAsync(this.SessionKey(session.SessionId), serSession);

            if (session.IsSupportUser)
            {
                transaction.StringSetAsync(this.UserKey(session.UserName), session.SessionId);
            }
            else
            {
                transaction.StringSetAsync(this.AccountKey(session.AccountId), session.SessionId);
            }

            transaction.SortedSetAddAsync(ExpirationSetKey, session.SessionId, unixNow);

            transaction.Execute();
        }

        public void Update(Session session)
        {
            this.Add(session);
        }

        public Session Get(int id)
        {
            var db = this.multiplexer.GetDatabase();

            var lastUpdated = db.SortedSetScore(ExpirationSetKey, id);

            // No information about session in the expiration set - return null
            if (!lastUpdated.HasValue)
            {
                return null;
            }

            var lastAlive = DateTimeOffset.FromUnixTimeSeconds((long)lastUpdated.Value);
            var utcNow = DateTimeOffset.UtcNow;

            // Session has expired and will be removed on the next expiration check - return null
            if (lastAlive + this.sessionTimeout < utcNow)
            {
                return null;
            }

            var serialized = db.StringGet(this.SessionKey(id));

            // Edge case - will only happen if the session gets deleted inbetween fetching its last update time
            // and retrieving the session itself
            if (serialized.IsNull)
            {
                return null;
            }

            return MessagePackSerializer.Deserialize<Session>(serialized);
        }

        public Session GetByUser(string userName)
        {
            return this.GetBySessionKey(this.UserKey(userName));
        }

        public Session GetByAccount(string accountId)
        {
            return this.GetBySessionKey(this.AccountKey(accountId));
        }

        public void Remove(int sessionId, string userName, string accountId)
        {
            var db = this.multiplexer.GetDatabase();
            db.SortedSetRemove(ExpirationSetKey, sessionId);
            db.KeyDelete(this.SessionKey(sessionId));

            var userKey = this.UserKey(userName);
            var accountKey = this.AccountKey(accountId);

            // Remove the user/account -> session ID key only if it still contains the same session ID
            this.RemoveKeyIfEquals(db, userKey, sessionId);
            this.RemoveKeyIfEquals(db, accountKey, sessionId);
        }

        public void RefreshSessionTimeouts(IEnumerable<Session> sessions)
        {
            var db = this.multiplexer.GetDatabase();

            var unixNow = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            var entriesToUpdate = sessions.Select(x => new SortedSetEntry(x.SessionId, unixNow)).ToArray();

            db.SortedSetAdd(ExpirationSetKey, entriesToUpdate);
        }

        public IEnumerable<Session> GetExpiredSessions()
        {
            var db = this.multiplexer.GetDatabase();
            var transaction = db.CreateTransaction();

            var maxTime = (DateTimeOffset.UtcNow - this.sessionTimeout).ToUnixTimeSeconds();

            // Retrieve the IDs to remove and remove them in one transaction - that way other instances of Axle
            // won't be able to produce duplicate TimeOut activities by retrieving the same range before it gets deleted
            var idsToRemoveTask = transaction.SortedSetRangeByScoreAsync(ExpirationSetKey, stop: maxTime);
            transaction.SortedSetRemoveRangeByScoreAsync(ExpirationSetKey, double.NegativeInfinity, maxTime);

            transaction.Execute();

            var idsToRemove = idsToRemoveTask.Result;

            var serializedSessions = db.StringGet(idsToRemove.Select(x => (RedisKey)this.SessionKey((int)x)).ToArray());

            return serializedSessions.Where(x => !x.IsNull).Select(x => MessagePackSerializer.Deserialize<Session>(x));
        }

        private Session GetBySessionKey(RedisKey sessionKey)
        {
            var sessionId = this.multiplexer.GetDatabase().StringGet(sessionKey);

            return sessionId.IsNull ? null : this.Get((int)sessionId);
        }

        private void RemoveKeyIfEquals(IDatabase db, RedisKey key, RedisValue value)
        {
            var transaction = db.CreateTransaction();

            transaction.AddCondition(Condition.StringEqual(key, value));
            transaction.KeyDeleteAsync(key);

            transaction.Execute();
        }

        private string UserKey(string user) => $"axle:users:{user}";

        private string AccountKey(string account) => $"axle:accounts:{account}";

        private string SessionKey(int session) => $"axle:sessions:{session}";
    }
}
