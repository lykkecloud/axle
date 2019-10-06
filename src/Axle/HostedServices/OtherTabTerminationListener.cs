// Copyright (c) 2019 Lykke Corp.
// See the LICENSE file in the project root for more information.

using JetBrains.Annotations;

namespace Axle.HostedServices
{
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Axle.Constants;
    using Axle.Dto;
    using Axle.Services;
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
            this.subscriber = multiplexer.GetSubscriber();
            this.hubConnectionService = hubConnectionService;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            return this.subscriber.SubscribeAsync(RedisChannels.OtherTabsTermination, 
                async (channel, value) => await this.HandleSessionTermination(channel, value));
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return this.subscriber.UnsubscribeAsync(RedisChannels.OtherTabsTermination, 
                async (channel, value) => await this.HandleSessionTermination(channel, value));
        }

        private async Task HandleSessionTermination(RedisChannel channel, RedisValue value)
        {
            var otherTabsNotification = MessagePackSerializer.Deserialize<TerminateOtherTabsNotification>(value);

            var connections = this.hubConnectionService.FindByAccessToken(otherTabsNotification.AccessToken);

            if (otherTabsNotification.OriginatingServiceId == AxleConstants.ServiceId)
            {
                connections = connections.Where(id => id != otherTabsNotification.OriginatingConnectionId);
            }

            await this.hubConnectionService.TerminateConnections(TerminateConnectionReason.DifferentTab,
                connections.ToArray());
        }
    }
}
