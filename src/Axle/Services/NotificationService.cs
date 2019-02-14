// Copyright (c) Lykke Corp.
// See the LICENSE file in the project root for more information.

namespace Axle.Services
{
    using System;
    using System.Collections.Generic;
    using StackExchange.Redis;

    public class NotificationService : INotificationService
    {
        private readonly IConnectionMultiplexer multiplexer;

        private readonly HashSet<Action<int>> callbacks = new HashSet<Action<int>>();

        public NotificationService(IConnectionMultiplexer multiplexer)
        {
            this.multiplexer = multiplexer;

            this.multiplexer.GetSubscriber().Subscribe(SessionTerminationNotifs, this.HandleSessionTermination);
        }

#pragma warning disable CA1710 // Event name should end in EventHandler
        public event Action<int> OnSessionTerminated
        {
            add { this.callbacks.Add(value); }
            remove { this.callbacks.Remove(value); }
        }
#pragma warning restore CA1710 // Event name should end in EventHandler

        private static string SessionTerminationNotifs => "axle:notifications:termsession";

        public void PublishSessionTermination(int sessionId)
        {
            this.multiplexer.GetDatabase().Publish(SessionTerminationNotifs, sessionId);
        }

        private void HandleSessionTermination(RedisChannel channel, RedisValue message)
        {
            foreach (var callback in this.callbacks)
            {
                callback((int)message);
            }
        }
    }
}
