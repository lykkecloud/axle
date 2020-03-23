// Copyright (c) 2019 Lykke Corp.
// See the LICENSE file in the project root for more information.

namespace Axle.HostedServices
{
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Axle.Constants;
    using Axle.Dto;
    using Axle.Extensions;
    using Axle.Services;
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
            this.subscriber = multiplexer.GetSubscriber();
            this.hubConnectionService = hubConnectionService;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            return this.subscriber.SubscribeAsync(
                RedisChannels.SessionTermination,
                async (channel, value) => await this.HandleSessionTermination(channel, value));
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return this.subscriber.UnsubscribeAsync(
                RedisChannels.SessionTermination,
                async (channel, value) => await this.HandleSessionTermination(channel, value));
        }

        private async Task HandleSessionTermination(RedisChannel channel, RedisValue value)
        {
            var terminateSessionNotification = MessagePackSerializer.Deserialize<TerminateSessionNotification>(value);

            var connections = this.hubConnectionService.FindBySessionId(terminateSessionNotification.SessionId);

            await this.hubConnectionService.TerminateConnections(
                terminateSessionNotification.Reason.ToTerminateConnectionReason(), connections.ToArray());
        }
    }
}
