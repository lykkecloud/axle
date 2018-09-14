// Copyright (c) Lykke Corp.
// See the LICENSE file in the project root for more information.

namespace Axle.Persistence
{
    using System.Collections.Generic;

    public interface IReadOnlyRepository<TId, TEntity>
    {
#pragma warning disable CA1716 // Identifiers should not match keywords
        TEntity Get(TId id);
    }
}
