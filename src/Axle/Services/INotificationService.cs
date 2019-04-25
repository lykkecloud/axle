// (c) Lykke Corporation 2019 - All rights reserved. No copying, adaptation, decompiling, distribution or any other form of use permitted.

namespace Axle.Services
{
    using Axle.Dto;

    public interface INotificationService
    {
        void PublishSessionTermination(TerminateSessionNotification terminateSessionNotification);

        void PublishOtherTabsTermination(TerminateOtherTabsNotification terminateOtherTabsNotification);
    }
}
