namespace Axle.Persistence
{
    using System.Collections.Generic;

    public interface IReadOnlyRepository<TId, TEntity>
    {
#pragma warning disable CA1716 // Identifiers should not match keywords
        TEntity Get(TId id);

        IEnumerable<TEntity> GetAll();
    }
}
