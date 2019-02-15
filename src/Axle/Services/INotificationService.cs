// Copyright (c) Lykke Corp.
// See the LICENSE file in the project root for more information.

namespace Axle.Services
{
    using System;

    public interface INotificationService
    {
#pragma warning disable CA1710 // Event name should end in EventHandler
        event Action<int> OnSessionTerminated;

        void PublishSessionTermination(int sessionId);
    }
}
