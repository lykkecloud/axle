namespace Axle.Persistence
{
    using System.Collections.Generic;

    public class InMemoryRepository<TId, TEntity> : IRepository<TId, TEntity>
    {
        private readonly IDictionary<TId, TEntity> repo = new Dictionary<TId, TEntity>();

        public void Add(TId id, TEntity entity)
        {
            this.repo.Add(id, entity);    
        }

        public TEntity Get(TId id)
        {
            if (this.repo.TryGetValue(id, out var entity))
            {
                return entity;
            }

            return default(TEntity);
        }

        public IEnumerable<TEntity> GetAll()
        {
            return this.repo.Values;
        }

        public void Remove(TId id)
        {
            this.repo.Remove(id);
        }
    }
}
