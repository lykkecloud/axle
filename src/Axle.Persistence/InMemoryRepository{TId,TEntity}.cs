// (c) Lykke Corporation 2019 - All rights reserved. No copying, adaptation, decompiling, distribution or any other form of use permitted.

namespace Axle.Persistence
{
    using System.Collections.Concurrent;

    public class InMemoryRepository<TId, TEntity> : IRepository<TId, TEntity>
    {
        private readonly ConcurrentDictionary<TId, TEntity> repo = new ConcurrentDictionary<TId, TEntity>();

        public void Add(TId id, TEntity entity)
        {
            this.repo.TryAdd(id, entity);
        }

        public TEntity Get(TId id)
        {
            if (this.repo.TryGetValue(id, out var entity))
            {
                return entity;
            }

            return default(TEntity);
        }

        public void Remove(TId id)
        {
            this.repo.TryRemove(id, out var _);
        }
    }
}
