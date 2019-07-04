// Copyright (c) 2019 Lykke Corp.
// See the LICENSE file in the project root for more information.

namespace Axle.Services
{
    using Axle.Dto;

    public interface INotificationService
    {
        void PublishSessionTermination(TerminateSessionNotification terminateSessionNotification);

        void PublishOtherTabsTermination(TerminateOtherTabsNotification terminateOtherTabsNotification);
    }
}
