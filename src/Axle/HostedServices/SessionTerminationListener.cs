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
    using MessagePack;
    using Microsoft.Extensions.Hosting;
    using StackExchange.Redis;

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
            return this.subscriber.SubscribeAsync(RedisChannels.SessionTermination, this.HandleSessionTermination);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return this.subscriber.UnsubscribeAsync(RedisChannels.SessionTermination, this.HandleSessionTermination);
        }

        private void HandleSessionTermination(RedisChannel channel, RedisValue value)
        {
            var terminateSessionNotification = MessagePackSerializer.Deserialize<TerminateSessionNotification>(value);

            var connections = this.hubConnectionService.FindBySessionId(terminateSessionNotification.SessionId);

            this.hubConnectionService.TerminateConnections(terminateSessionNotification.Reason.ToTerminateConnectionReason(), connections.ToArray());
        }
    }
}
