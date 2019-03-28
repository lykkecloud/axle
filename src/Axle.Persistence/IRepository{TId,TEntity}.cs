// (c) Lykke Corporation 2019 - All rights reserved. No copying, adaptation, decompiling, distribution or any other form of use permitted.

namespace Axle.Persistence
{
    public interface IRepository<TId, TEntity> : IReadOnlyRepository<TId, TEntity>
    {
        void Add(TId id, TEntity entity);

        void Remove(TId id);
    }
}
