// Copyright (c) Lykke Corp.
// See the LICENSE file in the project root for more information.

namespace Axle.Services
{
    using System;
    using Axle.Contracts;
    using Axle.Dto;

    public interface INotificationService
    {
#pragma warning disable CA1710 // Event name should end in EventHandler
        event Action<TerminateSessionNotification> OnSessionTerminated;

        void PublishSessionTermination(TerminateSessionNotification terminateSessionNotification);
    }
}
