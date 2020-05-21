// Copyright (c) 2019 Lykke Corp.
// See the LICENSE file in the project root for more information.

namespace Axle.HostedServices
{
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Constants;
    using Dto;
    using Services;
    using JetBrains.Annotations;
    using MessagePack;
    using Microsoft.Extensions.Hosting;
    using StackExchange.Redis;

    [UsedImplicitly]
    public class OtherTabTerminationListener : IHostedService
    {
        private readonly ISubscriber subscriber;
        private readonly IHubConnectionService hubConnectionService;

        public OtherTabTerminationListener(
            IConnectionMultiplexer multiplexer,
            IHubConnectionService hubConnectionService)
        {
            subscriber = multiplexer.GetSubscriber();
            this.hubConnectionService = hubConnectionService;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            return subscriber.SubscribeAsync(
                RedisChannels.OtherTabsTermination,
                async (channel, value) => await HandleSessionTermination(channel, value));
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return subscriber.UnsubscribeAsync(
                RedisChannels.OtherTabsTermination,
                async (channel, value) => await HandleSessionTermination(channel, value));
        }

        private async Task HandleSessionTermination(RedisChannel channel, RedisValue value)
        {
            var otherTabsNotification = MessagePackSerializer.Deserialize<TerminateOtherTabsNotification>(value);

            var connections = hubConnectionService.FindByAccessToken(otherTabsNotification.AccessToken);

            if (otherTabsNotification.OriginatingServiceId == AxleConstants.ServiceId)
            {
                connections = connections.Where(id => id != otherTabsNotification.OriginatingConnectionId);
            }

            await hubConnectionService.TerminateConnections(
                TerminateConnectionReason.DifferentTab,
                connections.ToArray());
        }
    }
}
