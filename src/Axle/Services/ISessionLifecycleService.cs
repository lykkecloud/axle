﻿// Copyright (c) Lykke Corp.
// See the LICENSE file in the project root for more information.

namespace Axle.Services
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface ISessionLifecycleService
    {
        #pragma warning disable CA1710 // Event name should end in EventHandler
        event Action<IEnumerable<string>> OnCloseConnections;

        Task OpenConnection(string connectionId, string userId, string clientId, string accessToken);

        void CloseConnection(string connectionId);
    }
}