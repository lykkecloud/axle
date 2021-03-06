﻿// Copyright (c) 2019 Lykke Corp.
// See the LICENSE file in the project root for more information.

namespace Axle.Persistence
{
    using System;
    using System.Collections.Generic;

    public interface IReadOnlyRepository<TId, TEntity>
    {
#pragma warning disable CA1716 // Identifiers should not match keywords
        TEntity Get(TId id);

        bool TryGet(TId id, out TEntity entity);

        IReadOnlyList<KeyValuePair<TId, TEntity>> GetAll();

        IEnumerable<KeyValuePair<TId, TEntity>> Find(Func<TEntity, bool> filter);
    }
}
