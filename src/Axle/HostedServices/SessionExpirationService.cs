// (c) Lykke Corporation 2019 - All rights reserved. No copying, adaptation, decompiling, distribution or any other form of use permitted.

namespace Axle.HostedServices
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Axle.Contracts;
    using Axle.Persistence;
    using Axle.Services;
    using Axle.Settings;
    using Lykke.Middlewares;
    using Lykke.Middlewares.Mappers;
    using Microsoft.Extensions.Logging;
    using Serilog;

    public sealed class SessionExpirationService : HostedServiceMiddleware, IDisposable
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
            this.cancellationTokenSource?.Dispose();
            this.cancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);

            this.expirationJob = this.DecorateAndHandle(() => this.RefreshTimeouts(this.cancellationTokenSource.Token));

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            this.cancellationTokenSource?.Cancel();
            return this.expirationJob ?? Task.CompletedTask;
        }

        public void Dispose()
        {
            this.cancellationTokenSource?.Cancel();
            this.cancellationTokenSource?.Dispose();
        }

        private async Task RefreshTimeouts(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                try
                {
                    try
                    {
                        await Task.Delay(this.sessionSettings.Timeout / 4, cancellationToken);
                    }
                    catch (TaskCanceledException)
                    {
                        break;
                    }

                    this.sessionRepository.RefreshSessionTimeouts(this.hubConnectionService.GetAllConnectedSessions());

                    var sessionsToTerminate = this.sessionRepository.GetExpiredSessions();

                    foreach (var session in sessionsToTerminate)
                    {
                        await this.sessionLifecycleService.TerminateSession(session, SessionActivityType.TimeOut);
                        this.logger.LogInformation($"Successfully timed out session: [{session.SessionId}] for user: [{session.UserName}]");
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
