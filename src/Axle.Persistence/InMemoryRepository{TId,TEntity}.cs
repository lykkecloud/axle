// Copyright (c) 2019 Lykke Corp.
// See the LICENSE file in the project root for more information.

namespace Axle.Persistence
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Linq;

    public class InMemoryRepository<TId, TEntity> : IRepository<TId, TEntity>
    {
        private readonly ConcurrentDictionary<TId, TEntity> repo = new ConcurrentDictionary<TId, TEntity>();

        public void Add(TId id, TEntity entity)
        {
            repo.TryAdd(id, entity);
        }

        public TEntity Get(TId id)
        {
            if (repo.TryGetValue(id, out var entity))
            {
                return entity;
            }

            return default;
        }

        public bool TryGet(TId id, out TEntity entity) => repo.TryGetValue(id, out entity);

        public IReadOnlyList<KeyValuePair<TId, TEntity>> GetAll()
        {
            return repo.ToArray();
        }

        public IEnumerable<KeyValuePair<TId, TEntity>> Find(Func<TEntity, bool> filter)
        {
            return repo.Where(x => filter(x.Value));
        }

        public void Remove(TId id)
        {
            repo.TryRemove(id, out var _);
        }
    }
}
