// Copyright (c) 2019 Lykke Corp.
// See the LICENSE file in the project root for more information.

namespace Axle.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Constants;
    using Dto;
    using Extensions;
    using Hubs;
    using Persistence;
    using Microsoft.AspNetCore.SignalR;
    using Microsoft.Extensions.Logging;

    public class HubConnectionService : IHubConnectionService
    {
        private readonly IRepository<string, HubCallerContext> connectionRepository;
        private readonly IRepository<string, int> sessionIdRepository;
        private readonly IHubContext<SessionHub> sessionHubContext;
        private readonly ISessionService sessionService;
        private readonly INotificationService notificationService;
        private readonly ILogger<HubConnectionService> logger;

        public HubConnectionService(
            IRepository<string, HubCallerContext> connectionRepository,
            IRepository<string, int> sessionIdRepository,
            IHubContext<SessionHub> sessionHubContext,
            ISessionService sessionService,
            INotificationService notificationService,
            ILogger<HubConnectionService> logger)
        {
            this.connectionRepository = connectionRepository;
            this.sessionIdRepository = sessionIdRepository;
            this.sessionHubContext = sessionHubContext;
            this.sessionService = sessionService;
            this.notificationService = notificationService;
            this.logger = logger;
        }

        public async Task OpenConnection(
            HubCallerContext context,
            string userName,
            string accountId,
            string clientId,
            string accessToken,
            bool isSupportUser)
        {
            var session = await sessionService.BeginSession(userName, accountId, clientId, accessToken, isSupportUser);

            connectionRepository.Add(context.ConnectionId, context);
            sessionIdRepository.Add(context.ConnectionId, session.SessionId);

            var terminateOtherTabs = new TerminateOtherTabsNotification
            {
                AccessToken = accessToken,
                OriginatingConnectionId = context.ConnectionId,
                OriginatingServiceId = AxleConstants.ServiceId
            };

            await notificationService.PublishOtherTabsTermination(terminateOtherTabs);
        }

        public void CloseConnection(string connectionId)
        {
            connectionRepository.Remove(connectionId);
            sessionIdRepository.Remove(connectionId);
        }

        public IEnumerable<int> GetAllConnectedSessions()
        {
            return sessionIdRepository.GetAll().Select(x => x.Value).Distinct();
        }

        public bool TryGetSessionId(string connectionId, out int sessionId) => sessionIdRepository.TryGet(connectionId, out sessionId);

        public IEnumerable<string> FindBySessionId(int sessionId)
        {
            return sessionIdRepository.Find(id => id == sessionId).Select(x => x.Key);
        }

        public IEnumerable<string> FindByAccessToken(string accessToken)
        {
            return connectionRepository.Find(context => context.GetHttpContext().Request.Query["access_token"].ToString() == accessToken).Select(x => x.Key);
        }

        public async Task TerminateConnections(TerminateConnectionReason reason, params string[] connectionIds)
        {
            switch (reason)
            {
                case TerminateConnectionReason.DifferentDevice:
                    await sessionHubContext.Clients.Clients(connectionIds)
                        .SendAsync("concurrentSessionTermination", StatusCode.IF_ATH_502, StatusCode.IF_ATH_502.ToMessage());
                    break;
                case TerminateConnectionReason.DifferentTab:
                    await sessionHubContext.Clients.Clients(connectionIds).SendAsync("concurrentTabTermination");
                    break;
                case TerminateConnectionReason.ByForce:
                    await sessionHubContext.Clients.Clients(connectionIds)
                        .SendAsync("byForceTermination", StatusCode.IF_ATH_503, StatusCode.IF_ATH_503.ToMessage());
                    break;
                default:
                    logger.LogInformation($"Not implemented for reason {reason}");
                    break;
            }

            foreach (var connectionId in connectionIds)
            {
                var connection = connectionRepository.Get(connectionId);

                if (connection == null)
                {
                    logger.LogWarning($"Connection with ID [{connectionId}] was not found and could not be terminated");
                }
                else
                {
                    connection.Abort();
                    logger.LogInformation($"Connection with ID [{connectionId}] was successfully terminated");
                }
            }
        }
    }
}
