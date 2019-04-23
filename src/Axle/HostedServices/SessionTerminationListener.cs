// (c) Lykke Corporation 2019 - All rights reserved. No copying, adaptation, decompiling, distribution or any other form of use permitted.

namespace Axle.HostedServices
{
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Axle.Dto;
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

        private static string SessionTerminationNotifs => "axle:notifications:termsession";

        public Task StartAsync(CancellationToken cancellationToken)
        {
            return this.subscriber.SubscribeAsync(SessionTerminationNotifs, this.HandleSessionTermination);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return this.subscriber.UnsubscribeAsync(SessionTerminationNotifs, this.HandleSessionTermination);
        }

        private void HandleSessionTermination(RedisChannel channel, RedisValue value)
        {
            var terminateSessionNotification = MessagePackSerializer.Deserialize<TerminateSessionNotification>(value);

            var connections = this.hubConnectionService.FindBySessionId(terminateSessionNotification.SessionId);

            this.hubConnectionService.TerminateConnections(terminateSessionNotification.Reason, connections.ToArray());
        }
    }
}
