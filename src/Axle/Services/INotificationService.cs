// Copyright (c) 2019 Lykke Corp.
// See the LICENSE file in the project root for more information.

namespace Axle.Services
{
    using System.Threading.Tasks;
    using Dto;

    public interface INotificationService
    {
        Task PublishSessionTermination(TerminateSessionNotification terminateSessionNotification);

        Task PublishOtherTabsTermination(TerminateOtherTabsNotification terminateOtherTabsNotification);
    }
}
