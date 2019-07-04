// Copyright (c) 2019 Lykke Corp.
// See the LICENSE file in the project root for more information.

namespace Axle.Persistence
{
    public interface IRepository<TId, TEntity> : IReadOnlyRepository<TId, TEntity>
    {
        void Add(TId id, TEntity entity);

        void Remove(TId id);
    }
}
