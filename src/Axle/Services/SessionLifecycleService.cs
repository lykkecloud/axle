// Copyright (c) Lykke Corp.
// See the LICENSE file in the project root for more information.

namespace Axle.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Axle.Dto;
    using Axle.Persistence;
    using Microsoft.Extensions.Logging;
    using Nito.AsyncEx;
    using Serilog;
    using StackExchange.Redis;

    public sealed class SessionLifecycleService : ISessionLifecycleService, IDisposable
    {
        private readonly ISessionRepository sessionRepository;
        private readonly ITokenRevocationService tokenRevocationService;
        private readonly INotificationService notificationService;
        private readonly ILogger<SessionLifecycleService> logger;
        private readonly TimeSpan sessionTimeout;

        private readonly HashSet<Action<IEnumerable<string>>> closeConnectionCallbacks = new HashSet<Action<IEnumerable<string>>>();
        private readonly Dictionary<string, Session> connectionSessionMap = new Dictionary<string, Session>();
        private readonly SemaphoreSlim slimLock = new SemaphoreSlim(1, 1);

        public SessionLifecycleService(
            ISessionRepository sessionRepository,
            ITokenRevocationService tokenRevocationService,
            INotificationService notificationService,
            ILogger<SessionLifecycleService> logger,
            TimeSpan sessionTimeout)
        {
            this.sessionRepository = sessionRepository;
            this.tokenRevocationService = tokenRevocationService;
            this.notificationService = notificationService;
            this.logger = logger;
            this.sessionTimeout = sessionTimeout;

            this.notificationService.OnSessionTerminated += this.HandleSessionTermination;

            this.RefreshTimeouts();
        }

#pragma warning disable CA1710 // Event name should end in EventHandler
        public event Action<IEnumerable<string>> OnCloseConnections
        {
            add { this.closeConnectionCallbacks.Add(value); }
            remove { this.closeConnectionCallbacks.Remove(value); }
        }
#pragma warning restore CA1710 // Event name should end in EventHandler

        public void CloseConnection(string connectionId)
        {
            this.slimLock.Wait();

            try
            {
                this.connectionSessionMap.Remove(connectionId);
            }
            finally
            {
                this.slimLock.Release();
            }
        }

        public async Task OpenConnection(string connectionId, string userId, string clientId, string accessToken)
        {
            Session userInfo;

            await this.slimLock.WaitAsync();

            try
            {
                userInfo = this.sessionRepository.GetByUser(userId);

                if (userInfo != null && userInfo.AccessToken == accessToken)
                {
                    this.connectionSessionMap.TryAdd(connectionId, userInfo);
                    return;
                }

                var rand = new Random();
                var sessionId = 0;

                do
                {
                    sessionId = rand.Next(int.MinValue, int.MaxValue);
                }
                while (this.sessionRepository.Get(sessionId) != null);

                var newState = new Session(userId, sessionId, accessToken, clientId);

                this.sessionRepository.Add(sessionId, newState);
                this.connectionSessionMap.TryAdd(connectionId, newState);

                if (userInfo != null)
                {
                    await this.TerminateSession(userInfo);
                }
            }
            finally
            {
                this.slimLock.Release();
            }
        }

        public async Task<TerminateSessionResponse> TerminateSession(string userId)
        {
            await this.slimLock.WaitAsync();

            try
            {
                var userInfo = this.sessionRepository.GetByUser(userId);

                if (userInfo == null)
                {
                    return new TerminateSessionResponse
                    {
                        Status = TerminateSessionStatus.NotFound,
                        ErrorMessage = $"No session found for the user: [{userId}]"
                    };
                }

                this.logger.LogInformation($"Terminating session: [{userInfo.SessionId}] for user: [{userId}]");

                await this.TerminateSession(userInfo);

                this.logger.LogInformation($"Successfully terminated session: [{userInfo.SessionId}] for user: [{userId}]");

                return new TerminateSessionResponse
                {
                    Status = TerminateSessionStatus.Terminated,
                    UserSessionId = userInfo.SessionId
                };
            }
            catch (Exception error)
            {
                this.logger.LogError(error, $"An unexpected error occurred while terminating session for user [{userId}]");

                return new TerminateSessionResponse
                {
                    Status = TerminateSessionStatus.Failed,
                    ErrorMessage = "An unknown error occurred while terminating user session"
                };
            }
            finally
            {
                this.slimLock.Release();
            }
        }

        public async Task TerminateSession(Session userInfo)
        {
            this.sessionRepository.Remove(userInfo.SessionId);
            await this.tokenRevocationService.RevokeAccessToken(userInfo.AccessToken, userInfo.ClientId);

            this.notificationService.PublishSessionTermination(userInfo.SessionId);
        }

        public void Dispose()
        {
            this.slimLock?.Dispose();
            GC.SuppressFinalize(this);
        }

        private async void RefreshTimeouts()
        {
            while (true)
            {
                try
                {
                    await Task.Delay(this.sessionTimeout / 4);

                    await this.slimLock.WaitAsync();

                    try
                    {
                        this.sessionRepository.RefreshSessionTimeouts(this.connectionSessionMap.Values);
                    }
                    finally
                    {
                        this.slimLock.Release();
                    }
                }
                catch (Exception e)
                {
                    Log.Error(e, "An error occured while trying to update session timeouts");
                }
            }
        }

        private void HandleSessionTermination(int sessionId)
        {
            this.slimLock.Wait();

            try
            {
                // Retrieve and remove the connections to the session we're closing
                var kvps = this.connectionSessionMap.Where(x => x.Value.SessionId == sessionId).ToArray();

                foreach (var kvp in kvps)
                {
                    this.connectionSessionMap.Remove(kvp.Key);
                }

                var connections = kvps.Select(x => x.Key).ToArray();

                foreach (var callback in this.closeConnectionCallbacks)
                {
                    callback(connections);
                }
            }
            finally
            {
                this.slimLock.Release();
            }
        }
    }
}
