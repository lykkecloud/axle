// (c) Lykke Corporation 2019 - All rights reserved. No copying, adaptation, decompiling, distribution or any other form of use permitted.

namespace Axle.Services
{
    using System.Threading.Tasks;
    using Axle.Contracts;
    using Axle.Dto;
    using Axle.Extensions;
    using Axle.Hubs;
    using Axle.Persistence;
    using Microsoft.AspNetCore.SignalR;
    using Microsoft.Extensions.Logging;

    public class HubConnectionService : IHubConnectionService
    {
        private readonly IRepository<string, HubCallerContext> connectionRepository;
        private readonly IHubContext<SessionHub> sessionHubContext;
        private readonly ILogger<HubConnectionService> logger;

        public HubConnectionService(
            IRepository<string, HubCallerContext> connectionRepository,
            IHubContext<SessionHub> sessionHubContext,
            ILogger<HubConnectionService> logger)
        {
            this.connectionRepository = connectionRepository;
            this.sessionHubContext = sessionHubContext;
            this.logger = logger;
        }

        public async Task TerminateConnections(SessionActivityType reason, params string[] connectionIds)
        {
            if (reason == SessionActivityType.DifferentDeviceTermination)
            {
                await this.sessionHubContext.Clients.Clients(connectionIds)
                                   .SendAsync("concurrentSessionTermination", StatusCode.IF_ATH_502, StatusCode.IF_ATH_502.ToMessage());
            }

            foreach (var connectionId in connectionIds)
            {
                var connection = this.connectionRepository.Get(connectionId);

                if (connection == null)
                {
                    this.logger.LogWarning($"Connection with ID [{connectionId}] was not found and could not be terminated");
                }
                else
                {
                    connection.Abort();
                    this.logger.LogInformation($"Connection with ID [{connectionId}] was successfully terminated");
                }
            }
        }
    }
}
