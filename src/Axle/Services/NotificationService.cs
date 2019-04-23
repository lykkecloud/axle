// (c) Lykke Corporation 2019 - All rights reserved. No copying, adaptation, decompiling, distribution or any other form of use permitted.

namespace Axle.Services
{
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

        private static string SessionTerminationNotifs => "axle:notifications:termsession";

        public void PublishSessionTermination(TerminateSessionNotification terminateSessionNotification)
        {
            var serTerminationSessionNotif = MessagePackSerializer.Serialize(terminateSessionNotification);
            this.multiplexer.GetDatabase().Publish(SessionTerminationNotifs, serTerminationSessionNotif);
        }
    }
}
