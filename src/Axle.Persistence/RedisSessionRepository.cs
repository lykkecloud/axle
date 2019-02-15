// Copyright (c) Lykke Corp.
// See the LICENSE file in the project root for more information.

namespace Axle.Persistence
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
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

        public void Add(int id, Session entity)
        {
            var serialized = MessagePackSerializer.Serialize(entity);

            var db = this.multiplexer.GetDatabase();

            var transaction = db.CreateTransaction();

            transaction.StringSetAsync(this.SessionKey(id), serialized, this.sessionTimeout);
            transaction.StringSetAsync(this.UserKey(entity.UserId), id, this.sessionTimeout);

            transaction.Execute();
        }

        public Session Get(int id)
        {
            var serialized = this.multiplexer.GetDatabase().StringGet(this.SessionKey(id));

            if (serialized.IsNull)
            {
                return null;
            }

            return MessagePackSerializer.Deserialize<Session>(serialized);
        }

        public Session GetByUser(string userId)
        {
            var userSession = this.multiplexer.GetDatabase().StringGet(this.UserKey(userId));

            return userSession.IsNull ? null : this.Get((int)userSession);
        }

        public void Remove(int id)
        {
            this.multiplexer.GetDatabase().KeyDelete(this.SessionKey(id));
        }

        public void RefreshSessionTimeouts(IEnumerable<Session> sessions)
        {
            var db = this.multiplexer.GetDatabase();

            db.WaitAll(this.RefreshKeyTasks(db, sessions).ToArray());
        }

        private IEnumerable<Task> RefreshKeyTasks(IDatabase db, IEnumerable<Session> sessions)
        {
            foreach (var session in sessions)
            {
                yield return db.KeyExpireAsync(this.UserKey(session.UserId), this.sessionTimeout);
                yield return db.KeyExpireAsync(this.SessionKey(session.SessionId), this.sessionTimeout);
            }
        }

        private string UserKey(string user) => $"axle:users:{user}";

        private string SessionKey(int session) => $"axle:sessions:{session}";
    }
}
