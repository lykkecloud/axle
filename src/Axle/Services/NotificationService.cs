// (c) Lykke Corporation 2019 - All rights reserved. No copying, adaptation, decompiling, distribution or any other form of use permitted.

namespace Axle.Services
{
    using System;
    using System.Collections.Generic;
    using Axle.Contracts;
    using Axle.Dto;
    using MessagePack;
    using StackExchange.Redis;

    public class NotificationService : INotificationService
    {
        private readonly IConnectionMultiplexer multiplexer;

        private readonly HashSet<Action<TerminateSessionNotification>> callbacks = new HashSet<Action<TerminateSessionNotification>>();

        public NotificationService(IConnectionMultiplexer multiplexer)
        {
            this.multiplexer = multiplexer;

            this.multiplexer.GetSubscriber().Subscribe(SessionTerminationNotifs, this.HandleSessionTermination);
        }

#pragma warning disable CA1710 // Event name should end in EventHandler
        public event Action<TerminateSessionNotification> OnSessionTerminated
        {
            add { this.callbacks.Add(value); }
            remove { this.callbacks.Remove(value); }
        }
#pragma warning restore CA1710 // Event name should end in EventHandler

        private static string SessionTerminationNotifs => "axle:notifications:termsession";

        public void PublishSessionTermination(TerminateSessionNotification terminateSessionNotification)
        {
            var serTerminationSessionNotif = MessagePackSerializer.Serialize(terminateSessionNotification);
            this.multiplexer.GetDatabase().Publish(SessionTerminationNotifs, serTerminationSessionNotif);
        }

        private void HandleSessionTermination(RedisChannel channel, RedisValue message)
        {
            var result = MessagePackSerializer.Deserialize<TerminateSessionNotification>(message);

            foreach (var callback in this.callbacks)
            {
                callback(result);
            }
        }
    }
}
