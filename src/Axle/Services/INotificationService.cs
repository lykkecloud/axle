// (c) Lykke Corporation 2019 - All rights reserved. No copying, adaptation, decompiling, distribution or any other form of use permitted.

namespace Axle.Services
{
    using System;
    using Axle.Contracts;
    using Axle.Dto;

    public interface INotificationService
    {
#pragma warning disable CA1710 // Event name should end in EventHandler
        event Action<TerminateSessionNotification> OnSessionTerminated;

        event Action<int> OnBehalfChanged;

        void PublishSessionTermination(TerminateSessionNotification terminateSessionNotification);

        void PublishOnBehalfChange(int sessionId);
    }
}
