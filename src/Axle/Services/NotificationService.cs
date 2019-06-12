// (c) Lykke Corporation 2019 - All rights reserved. No copying, adaptation, decompiling, distribution or any other form of use permitted.

namespace Axle.Services
{
    using Axle.Constants;
    using Axle.Dto;
    using MessagePack;
    using StackExchange.Redis;

    public class NotificationService : INotificationService
    {
        private readonly IConnectionMultiplexer multiplexer;

        public NotificationService(IConnectionMultiplexer multiplexer)
        {
            this.multiplexer = multiplexer;
        }

        public void PublishSessionTermination(TerminateSessionNotification terminateSessionNotification)
        {
            var serTerminationSessionNotif = MessagePackSerializer.Serialize(terminateSessionNotification);
            this.multiplexer.GetDatabase().Publish(RedisChannels.SessionTermination, serTerminationSessionNotif);
        }

        public void PublishOtherTabsTermination(TerminateOtherTabsNotification terminateOtherTabsNotification)
        {
            var serializedNotif = MessagePackSerializer.Serialize(terminateOtherTabsNotification);
            this.multiplexer.GetDatabase().Publish(RedisChannels.OtherTabsTermination, serializedNotif);
        }
    }
}
