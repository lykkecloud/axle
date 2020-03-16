// Copyright (c) 2019 Lykke Corp.
// See the LICENSE file in the project root for more information.

namespace Axle.Services
{
    using System.Threading.Tasks;
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

        public async Task PublishSessionTermination(TerminateSessionNotification terminateSessionNotification)
        {
            var serializedNotification = MessagePackSerializer.Serialize(terminateSessionNotification);
            await this.multiplexer.GetDatabase().PublishAsync(RedisChannels.SessionTermination, serializedNotification);
        }

        public async Task PublishOtherTabsTermination(TerminateOtherTabsNotification terminateOtherTabsNotification)
        {
            var serializedNotification = MessagePackSerializer.Serialize(terminateOtherTabsNotification);
            await this.multiplexer.GetDatabase().PublishAsync(RedisChannels.OtherTabsTermination, serializedNotification);
        }
    }
}
