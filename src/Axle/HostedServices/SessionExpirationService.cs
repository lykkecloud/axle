// Copyright (c) 2019 Lykke Corp.
// See the LICENSE file in the project root for more information.

namespace Axle.HostedServices
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Contracts;
    using Persistence;
    using Services;
    using Settings;
    using Lykke.Middlewares;
    using Lykke.Middlewares.Mappers;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;
    using Serilog;

    public sealed class SessionExpirationService : HostedServiceMiddleware, IHostedService, IDisposable
    {
        private readonly ISessionRepository sessionRepository;
        private readonly ISessionService sessionLifecycleService;
        private readonly IHubConnectionService hubConnectionService;
        private readonly ILogger<SessionExpirationService> logger;
        private readonly SessionSettings sessionSettings;

        private Task expirationJob;
        private CancellationTokenSource cancellationTokenSource;

        public SessionExpirationService(
            ILogLevelMapper logLevelMapper,
            ISessionRepository sessionRepository,
            ISessionService sessionLifecycleService,
            IHubConnectionService hubConnectionService,
            ILogger<SessionExpirationService> logger,
            SessionSettings sessionSettings)
            : base(logLevelMapper, logger)
        {
            this.sessionRepository = sessionRepository;
            this.sessionLifecycleService = sessionLifecycleService;
            this.hubConnectionService = hubConnectionService;
            this.logger = logger;
            this.sessionSettings = sessionSettings;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            cancellationTokenSource?.Dispose();
            cancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);

            expirationJob = DecorateAndHandle(() => RefreshTimeouts(cancellationTokenSource.Token));

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            cancellationTokenSource?.Cancel();
            return expirationJob ?? Task.CompletedTask;
        }

        public void Dispose()
        {
            cancellationTokenSource?.Cancel();
            cancellationTokenSource?.Dispose();
        }

        private async Task RefreshTimeouts(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                try
                {
                    try
                    {
                        await Task.Delay(sessionSettings.Timeout / 4, cancellationToken);
                    }
                    catch (TaskCanceledException)
                    {
                        break;
                    }

                    await sessionRepository.RefreshSessionTimeouts(hubConnectionService.GetAllConnectedSessions());

                    var sessionsToTerminate = await sessionRepository.GetExpiredSessions();

                    foreach (var session in sessionsToTerminate)
                    {
                        await sessionLifecycleService.TerminateSession(session, SessionActivityType.TimeOut);
                        logger.LogInformation($"Successfully timed out session: [{session.SessionId}] for user: [{session.UserName}]");
                    }
                }
                catch (Exception e)
                {
                    Log.Error(e, "An error occured while trying to update session timeouts");
                }
            }
        }
    }
}
