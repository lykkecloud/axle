// (c) Lykke Corporation 2019 - All rights reserved. No copying, adaptation, decompiling, distribution or any other form of use permitted.

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
            return this.subscriber.SubscribeAsync(RedisChannels.OtherTabsTermination, this.HandleSessionTermination);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return this.subscriber.UnsubscribeAsync(RedisChannels.OtherTabsTermination, this.HandleSessionTermination);
        }

        private void HandleSessionTermination(RedisChannel channel, RedisValue value)
        {
            var otherTabsNotif = MessagePackSerializer.Deserialize<TerminateOtherTabsNotification>(value);

            var connections = this.hubConnectionService.FindByAccessToken(otherTabsNotif.AccessToken);

            if (otherTabsNotif.OriginatingServiceId == AxleConstants.ServiceId)
            {
                connections = connections.Where(id => id != otherTabsNotif.OriginatingConnectionId);
            }

            this.hubConnectionService.TerminateConnections(TerminateConnectionReason.DifferentTab, connections.ToArray());
        }
    }
}
