namespace Axle.Persistence
{
    using System.Collections.Generic;

    public interface IRepository<TId, TEntity>
    {
        void Add(TId id, TEntity entity);

        TEntity Get(TId id);

        IEnumerable<TEntity> GetAll();

        void Remove(TId id);
    }
}
