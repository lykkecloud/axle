// (c) Lykke Corporation 2019 - All rights reserved. No copying, adaptation, decompiling, distribution or any other form of use permitted.

namespace Axle.Persistence
{
    using System.Collections.Generic;

    public interface IReadOnlyRepository<TId, TEntity>
    {
#pragma warning disable CA1716 // Identifiers should not match keywords
        TEntity Get(TId id);
    }
}
