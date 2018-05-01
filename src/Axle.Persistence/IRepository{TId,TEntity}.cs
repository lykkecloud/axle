namespace Axle.Persistence
{
    public interface IRepository<TId, TEntity> : IReadOnlyRepository<TId, TEntity>
    {
        void Add(TId id, TEntity entity);

        void Remove(TId id);
    }
}
