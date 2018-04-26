namespace Axle.Persistence
{
    using System.Collections.Generic;

    public interface IRepository<TId, TEntity>
    {
        void Add(TId id, TEntity entity);

#pragma warning disable CA1716 // Identifiers should not match keywords
        TEntity Get(TId id);

        IEnumerable<TEntity> GetAll();

        void Remove(TId id);
    }
}
