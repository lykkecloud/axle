// Copyright (c) 2019 Lykke Corp.
// See the LICENSE file in the project root for more information.

namespace Axle.Persistence
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using MessagePack;
    using Microsoft.Extensions.Logging;
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
            this.logger.LogDebug($"Trying to add new session: {nameof(session.AccountId)}:{session.AccountId}, {nameof(session.SessionId)}: {session.SessionId}..");

            var serSession = MessagePackSerializer.Serialize(session);
            var unixNow = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

            var db = this.multiplexer.GetDatabase();

            var transaction = db.CreateTransaction();

            try
            {
#pragma warning disable 4014
                transaction.StringSetAsync(SessionKey(session.SessionId), serSession);

                if (session.IsSupportUser)
                {
                    transaction.StringSetAsync(UserKey(session.UserName), session.SessionId);
                }
                else
                {
                    transaction.StringSetAsync(AccountKey(session.AccountId), session.SessionId);
                }

                transaction.SortedSetAddAsync(ExpirationSetKey, session.SessionId, unixNow);
#pragma warning restore 4014
                if (!await transaction.ExecuteAsync())
                {
                    throw new Exception($"Transaction to add new session was not committed.");
                }
            }
            catch (Exception exception)
            {
                this.logger.LogError(exception, $"Error occured while creating new session: {nameof(session.AccountId)}:{session.AccountId}, {nameof(session.SessionId)}: {session.SessionId}.");
            }
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

        public async Task<int?> GetSessionIdByUser(string userName)
        {
            return await this.GetSessionIdBySessionKey(UserKey(userName));
        }

        public async Task<Session> GetByAccount(string accountId)
        {
            return await this.GetBySessionKey(AccountKey(accountId));
        }

        public async Task<int?> GetSessionIdByAccount(string accountId)
        {
            return await this.GetSessionIdBySessionKey(AccountKey(accountId));
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
            var maxTime = (DateTimeOffset.UtcNow - this.sessionTimeout).ToUnixTimeSeconds();

            var transaction = db.CreateTransaction();

            // Retrieve the IDs to remove and remove them in one transaction - that way other instances of Axle
            // won't be able to produce duplicate TimeOut activities by retrieving the same range before it gets deleted
            var idsToRemoveTask = transaction.SortedSetRangeByScoreAsync(ExpirationSetKey, stop: maxTime);
#pragma warning disable 4014
            transaction.SortedSetRemoveRangeByScoreAsync(ExpirationSetKey, double.NegativeInfinity, maxTime);
#pragma warning restore 4014

            if (!await transaction.ExecuteAsync())
            {
                throw new Exception($"{nameof(RedisSessionRepository)}:{nameof(GetExpiredSessions)} failed to commit transaction.");
            }

            var serializedSessions = await db.StringGetAsync(
                (await idsToRemoveTask).Select(x => (RedisKey) SessionKey((int) x)).ToArray());

            return serializedSessions.Where(x => !x.IsNull).Select(x => MessagePackSerializer.Deserialize<Session>(x));
        }

        private static string UserKey(string user) => $"axle:users:{user}";

        private static string AccountKey(string account) => $"axle:accounts:{account}";

        private static string SessionKey(int session) => $"axle:sessions:{session}";

        private async Task<int?> GetSessionIdBySessionKey(RedisKey sessionKey)
        {
            string sessionId = await this.multiplexer.GetDatabase().StringGetAsync(sessionKey);

            this.logger.LogDebug($"{nameof(RedisSessionRepository)}:{nameof(GetSessionIdBySessionKey)}:{sessionKey} returned {(string.IsNullOrEmpty(sessionId) ? "empty string" : sessionId)}");

            return string.IsNullOrEmpty(sessionId) ? (int?)null : int.Parse(sessionId);
        }

        private async Task<Session> GetBySessionKey(RedisKey sessionKey)
        {
            var sessionId = await GetSessionIdBySessionKey(sessionKey);

            return !sessionId.HasValue ? null : await this.Get(sessionId.Value);
        }

        private async Task RemoveKeyIfEquals(IDatabase db, RedisKey key, RedisValue value)
        {
            var transaction = db.CreateTransaction();

            transaction.AddCondition(Condition.StringEqual(key, value));
#pragma warning disable 4014
            transaction.KeyDeleteAsync(key);
#pragma warning restore 4014

            if (!await transaction.ExecuteAsync())
            {
                this.logger.LogWarning($"{nameof(RedisSessionRepository)}:{nameof(RemoveKeyIfEquals)}: failed to commit transaction.");
            }
        }
    }
}
