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

        private readonly HashSet<Action<TerminateSessionNotification>> terminateSessionCallbacks = new HashSet<Action<TerminateSessionNotification>>();
        private readonly HashSet<Action<int>> onBehalfChangeCallbacks = new HashSet<Action<int>>();

        public NotificationService(IConnectionMultiplexer multiplexer)
        {
            this.multiplexer = multiplexer;

            var subscriber = this.multiplexer.GetSubscriber();

            subscriber.Subscribe(SessionTerminationNotifs, this.HandleSessionTermination);
            subscriber.Subscribe(OnBehalfChangeNotifs, this.HandleOnBehalfChange);
        }

#pragma warning disable CA1710 // Event name should end in EventHandler
        public event Action<TerminateSessionNotification> OnSessionTerminated
        {
            add { this.terminateSessionCallbacks.Add(value); }
            remove { this.terminateSessionCallbacks.Remove(value); }
        }

        public event Action<int> OnBehalfChanged
        {
            add { this.onBehalfChangeCallbacks.Add(value); }
            remove { this.onBehalfChangeCallbacks.Remove(value); }
        }
#pragma warning restore CA1710 // Event name should end in EventHandler

        private static string SessionTerminationNotifs => "axle:notifications:termsession";

        private static string OnBehalfChangeNotifs => "axle:notifications:onbehalfchange";

        public void PublishSessionTermination(TerminateSessionNotification terminateSessionNotification)
        {
            var serTerminationSessionNotif = MessagePackSerializer.Serialize(terminateSessionNotification);
            this.multiplexer.GetDatabase().Publish(SessionTerminationNotifs, serTerminationSessionNotif);
        }

        public void PublishOnBehalfChange(int sessionId)
        {
            this.multiplexer.GetDatabase().Publish(OnBehalfChangeNotifs, sessionId);
        }

        private void HandleSessionTermination(RedisChannel channel, RedisValue message)
        {
            var result = MessagePackSerializer.Deserialize<TerminateSessionNotification>(message);

            foreach (var callback in this.terminateSessionCallbacks)
            {
                callback(result);
            }
        }

        private void HandleOnBehalfChange(RedisChannel channel, RedisValue value)
        {
            var sessionId = (int)value;

            foreach (var callback in this.onBehalfChangeCallbacks)
            {
                callback(sessionId);
            }
        }
    }
}
