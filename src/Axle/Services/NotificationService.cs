// Copyright (c) Lykke Corp.
// See the LICENSE file in the project root for more information.

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
            foreach (var callback in this.callbacks)
            {
                var result = MessagePackSerializer.Deserialize<TerminateSessionNotification>(message);
                callback(result);
            }
        }
    }
}
