// Copyright (c) 2019 Lykke Corp.
// See the LICENSE file in the project root for more information.

namespace Axle.HostedServices
{
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Constants;
    using Dto;
    using Extensions;
    using Services;
    using JetBrains.Annotations;
    using MessagePack;
    using Microsoft.Extensions.Hosting;
    using StackExchange.Redis;

    [UsedImplicitly]
    public class SessionTerminationListener : IHostedService
    {
        private readonly ISubscriber subscriber;
        private readonly IHubConnectionService hubConnectionService;

        public SessionTerminationListener(
            IConnectionMultiplexer multiplexer,
            IHubConnectionService hubConnectionService)
        {
            subscriber = multiplexer.GetSubscriber();
            this.hubConnectionService = hubConnectionService;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            return subscriber.SubscribeAsync(
                RedisChannels.SessionTermination,
                async (channel, value) => await HandleSessionTermination(channel, value));
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return subscriber.UnsubscribeAsync(
                RedisChannels.SessionTermination,
                async (channel, value) => await HandleSessionTermination(channel, value));
        }

        private async Task HandleSessionTermination(RedisChannel channel, RedisValue value)
        {
            var terminateSessionNotification = MessagePackSerializer.Deserialize<TerminateSessionNotification>(value);

            var connections = hubConnectionService.FindBySessionId(terminateSessionNotification.SessionId);

            await hubConnectionService.TerminateConnections(
                terminateSessionNotification.Reason.ToTerminateConnectionReason(), connections.ToArray());
        }
    }
}
