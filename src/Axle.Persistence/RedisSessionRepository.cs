// Copyright (c) 2019 Lykke Corp.
// See the LICENSE file in the project root for more information.

using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

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
        private readonly ILogger<RedisSessionRepository> logger;

        public RedisSessionRepository(IConnectionMultiplexer multiplexer, TimeSpan sessionTimeout, ILogger<RedisSessionRepository> logger)
        {
            this.multiplexer = multiplexer;
            this.sessionTimeout = sessionTimeout;
            this.logger = logger;
        }

        private static string ExpirationSetKey => "axle:expirations";

        public async Task Add(Session session)
        {
            var serSession = MessagePackSerializer.Serialize(session);
            var unixNow = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

            var db = this.multiplexer.GetDatabase();

            var transaction = db.CreateTransaction();

            await transaction.StringSetAsync(SessionKey(session.SessionId), serSession);

            if (session.IsSupportUser)
            {
                await transaction.StringSetAsync(UserKey(session.UserName), session.SessionId);
            }
            else
            {
                await transaction.StringSetAsync(AccountKey(session.AccountId), session.SessionId);
            }

            await transaction.SortedSetAddAsync(ExpirationSetKey, session.SessionId, unixNow);

            await transaction.ExecuteAsync();
        }

        public async Task Update(Session session)
        {
            await this.Add(session);
        }

        public async Task<Session> Get(int id)
        {
            var db = this.multiplexer.GetDatabase();

            var lastUpdated = await db.SortedSetScoreAsync(ExpirationSetKey, id);

            // No information about session in the expiration set - return null
            if (!lastUpdated.HasValue)
            {
                this.logger.LogDebug($"{nameof(RedisSessionRepository)}:{nameof(Get)}:{id}: No information about session in the expiration set - return null");
                return null;
            }

            var lastAlive = DateTimeOffset.FromUnixTimeSeconds((long)lastUpdated.Value);
            var utcNow = DateTimeOffset.UtcNow;

            // Session has expired and will be removed on the next expiration check - return null
            if (lastAlive + this.sessionTimeout < utcNow)
            {
                this.logger.LogDebug($"{nameof(RedisSessionRepository)}:{nameof(Get)}:{id}: Session has expired and will be removed on the next expiration check - return null");
                return null;
            }

            var serialized = await db.StringGetAsync(SessionKey(id));

            // Edge case - will only happen if the session gets deleted in between fetching its last update time
            // and retrieving the session itself
            if (serialized.IsNull)
            {
                this.logger.LogDebug($"{nameof(RedisSessionRepository)}:{nameof(Get)}:{id}: Edge case - will only happen if the session gets deleted in between fetching its last update time and retrieving the session itself");
                return null;
            }

            return MessagePackSerializer.Deserialize<Session>(serialized);
        }

        public async Task<Session> GetByUser(string userName)
        {
            return await this.GetBySessionKey(UserKey(userName));
        }

        public async Task<Session> GetByAccount(string accountId)
        {
            return await this.GetBySessionKey(AccountKey(accountId));
        }

        public async Task Remove(int sessionId, string userName, string accountId)
        {
            var db = this.multiplexer.GetDatabase();
            await db.SortedSetRemoveAsync(ExpirationSetKey, sessionId);
            await db.KeyDeleteAsync(SessionKey(sessionId));

            // Remove the user/account -> session ID key only if it still contains the same session ID
            await RemoveKeyIfEquals(db, UserKey(userName), sessionId);
            await RemoveKeyIfEquals(db, AccountKey(accountId), sessionId);
        }

        public async Task RefreshSessionTimeouts(IEnumerable<int> sessions)
        {
            var db = this.multiplexer.GetDatabase();

            var unixNow = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            var entriesToUpdate = sessions.Select(session => new SortedSetEntry(session, unixNow)).ToArray();

            await db.SortedSetAddAsync(ExpirationSetKey, entriesToUpdate);
        }

        public async Task<IEnumerable<Session>> GetExpiredSessions()
        {
            var db = this.multiplexer.GetDatabase();
            var transaction = db.CreateTransaction();

            var maxTime = (DateTimeOffset.UtcNow - this.sessionTimeout).ToUnixTimeSeconds();

            // Retrieve the IDs to remove and remove them in one transaction - that way other instances of Axle
            // won't be able to produce duplicate TimeOut activities by retrieving the same range before it gets deleted
            var idsToRemoveTask = transaction.SortedSetRangeByScoreAsync(ExpirationSetKey, stop: maxTime);
            await transaction.SortedSetRemoveRangeByScoreAsync(ExpirationSetKey, double.NegativeInfinity, maxTime);

            await transaction.ExecuteAsync();

            var idsToRemove = await idsToRemoveTask;

            var serializedSessions = await db.StringGetAsync(
                idsToRemove.Select(x => (RedisKey)SessionKey((int)x)).ToArray());

            return serializedSessions.Where(x => !x.IsNull).Select(x => MessagePackSerializer.Deserialize<Session>(x));
        }

        private async Task<Session> GetBySessionKey(RedisKey sessionKey)
        {
            var sessionId = await this.multiplexer.GetDatabase().StringGetAsync(sessionKey);

            return sessionId.IsNull ? null : await this.Get((int) sessionId);
        }

        private static async Task RemoveKeyIfEquals(IDatabase db, RedisKey key, RedisValue value)
        {
            var transaction = db.CreateTransaction();

            transaction.AddCondition(Condition.StringEqual(key, value));
            await transaction.KeyDeleteAsync(key);

            await transaction.ExecuteAsync();
        }

        private static string UserKey(string user) => $"axle:users:{user}";

        private static string AccountKey(string account) => $"axle:accounts:{account}";

        private static string SessionKey(int session) => $"axle:sessions:{session}";
    }
}
