// (c) Lykke Corporation 2019 - All rights reserved. No copying, adaptation, decompiling, distribution or any other form of use permitted.

namespace Axle.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Axle.Contracts;
    using Axle.Dto;
    using Axle.Extensions;
    using Axle.Persistence;
    using Axle.Settings;
    using Microsoft.Extensions.Logging;
    using Serilog;

    public sealed class SessionLifecycleService : ISessionLifecycleService, IDisposable
    {
        private readonly ISessionRepository sessionRepository;
        private readonly ITokenRevocationService tokenRevocationService;
        private readonly INotificationService notificationService;
        private readonly IActivityService activityService;
        private readonly IAccountsService accountsService;
        private readonly IHubConnectionService hubConnectionService;
        private readonly ILogger<SessionLifecycleService> logger;
        private readonly TimeSpan sessionTimeout;

        private readonly Dictionary<string, Session> connectionSessionMap = new Dictionary<string, Session>();
        private readonly SemaphoreSlim slimLock = new SemaphoreSlim(1, 1);

        public SessionLifecycleService(
            ISessionRepository sessionRepository,
            ITokenRevocationService tokenRevocationService,
            INotificationService notificationService,
            IActivityService activityService,
            IAccountsService accountsService,
            IHubConnectionService hubConnectionService,
            ILogger<SessionLifecycleService> logger,
            SessionSettings sessionSettings)
        {
            this.sessionRepository = sessionRepository;
            this.tokenRevocationService = tokenRevocationService;
            this.notificationService = notificationService;
            this.activityService = activityService;
            this.accountsService = accountsService;
            this.hubConnectionService = hubConnectionService;
            this.logger = logger;
            this.sessionTimeout = sessionSettings.Timeout;

            this.notificationService.OnSessionTerminated += this.HandleSessionTermination;
            this.notificationService.OnBehalfChanged += this.HandleOnBehalfChange;

            this.RefreshTimeouts();
        }

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

        public async Task OpenConnection(
            string connectionId,
            string userName,
            string accountId,
            string clientId,
            string accessToken,
            bool isSupportUser)
        {
            Session userInfo;

            await this.slimLock.WaitAsync();

            try
            {
                userInfo = isSupportUser ? this.sessionRepository.GetByUser(userName) : this.sessionRepository.GetByAccount(accountId);

                if (userInfo != null && userInfo.AccessToken == accessToken)
                {
                    this.connectionSessionMap.TryAdd(connectionId, userInfo);
                    return;
                }

                var sessionId = this.GenerateSessionId();

                var newSession = new Session(userName, sessionId, accountId, accessToken, clientId, isSupportUser);

                this.sessionRepository.Add(newSession);
                this.connectionSessionMap.TryAdd(connectionId, newSession);

                if (!newSession.IsSupportUser)
                {
                    await this.activityService.PublishActivity(newSession, SessionActivityType.Login);
                }

                if (userInfo != null)
                {
                    await this.TerminateSession(userInfo, SessionActivityType.DifferentDeviceTermination);
                    this.logger.LogWarning(StatusCode.IF_ATH_502.ToMessage());
                }

                this.logger.LogWarning(StatusCode.IF_ATH_501.ToMessage());
            }
            finally
            {
                this.slimLock.Release();
            }
        }

        public async Task<OnBehalfChangeResponse> UpdateOnBehalfState(string connectionId, string onBehalfAccount)
        {
            this.slimLock.Wait();

            try
            {
                if (!this.connectionSessionMap.TryGetValue(connectionId, out Session session))
                {
                    return OnBehalfChangeResponse.Fail($"Unknown connection ID [{connectionId}]");
                }

                if (!session.IsSupportUser)
                {
                    return OnBehalfChangeResponse.Fail($"User [{session.UserName}] is not a support user");
                }

                if (session.AccountId == onBehalfAccount)
                {
                    return OnBehalfChangeResponse.Fail($"Cannot switch to the same on behalf account");
                }

                string onBehalfOwner = null;

                if (!string.IsNullOrEmpty(onBehalfAccount))
                {
                    onBehalfOwner = await this.accountsService.GetAccountOwnerUserName(onBehalfAccount);

                    if (string.IsNullOrEmpty(onBehalfOwner))
                    {
                        return OnBehalfChangeResponse.Fail($"Account [{onBehalfAccount}] was not found");
                    }
                }

                var newSession = new Session(session.UserName, session.SessionId, onBehalfAccount, session.AccessToken, session.ClientId, session.IsSupportUser);

                this.sessionRepository.Update(newSession);

                if (!string.IsNullOrEmpty(session.AccountId))
                {
                    var accountOwner = await this.accountsService.GetAccountOwnerUserName(session.AccountId);
                    var sessionActivity = new SessionActivity(SessionActivityType.OnBehalfSupportDisconnected, session.SessionId, accountOwner, session.AccountId);

                    await this.activityService.PublishActivity(sessionActivity);
                }

                if (!string.IsNullOrEmpty(onBehalfAccount))
                {
                    await this.activityService.PublishActivity(new SessionActivity(SessionActivityType.OnBehalfSupportConnected, session.SessionId, onBehalfOwner, onBehalfAccount));
                }

                this.notificationService.PublishOnBehalfChange(session.SessionId);

                return OnBehalfChangeResponse.Success();
            }
            finally
            {
                this.slimLock.Release();
            }
        }

        public async Task<TerminateSessionResponse> TerminateSession(
            string userName,
            string accountId,
            bool isSupportUser,
            SessionActivityType reason = SessionActivityType.ManualTermination)
        {
            await this.slimLock.WaitAsync();

            try
            {
                var userInfo = this.sessionRepository.GetByUser(userName);

                if (userInfo == null && !isSupportUser && !string.IsNullOrEmpty(accountId))
                {
                    userInfo = this.sessionRepository.GetByAccount(accountId);
                }

                if (userInfo == null)
                {
                    return new TerminateSessionResponse
                    {
                        Status = TerminateSessionStatus.NotFound,
                        ErrorMessage = $"No session found for the user: [{userName}] and account: [{accountId}]"
                    };
                }

                this.logger.LogInformation($"Terminating session: [{userInfo.SessionId}] for user: [{userName}] and account: [{accountId}]");

                await this.TerminateSession(userInfo, reason);

                if (reason == SessionActivityType.ManualTermination)
                {
                    this.logger.LogWarning(StatusCode.WN_ATH_701.ToMessage());
                }

                this.logger.LogInformation($"Successfully terminated session: [{userInfo.SessionId}] for user: [{userName}] and account: [{accountId}]");

                return new TerminateSessionResponse
                {
                    Status = TerminateSessionStatus.Terminated,
                    UserSessionId = userInfo.SessionId
                };
            }
            catch (Exception error)
            {
                this.logger.LogError(error, $"An unexpected error occurred while terminating session for user [{userName}] and account: [{accountId}]");

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

        public async Task TerminateSession(Session userInfo, SessionActivityType reason)
        {
            this.sessionRepository.Remove(userInfo.SessionId, userInfo.UserName, userInfo.AccountId);

            if (reason != SessionActivityType.SignOut)
            {
                await this.tokenRevocationService.RevokeAccessToken(userInfo.AccessToken, userInfo.ClientId);
            }

            this.notificationService.PublishSessionTermination(new TerminateSessionNotification() { SessionId = userInfo.SessionId, Reason = reason });

            // Support user activities are not required currently
            if (!userInfo.IsSupportUser)
            {
                await this.activityService.PublishActivity(userInfo, reason);
            }
        }

        public int GenerateSessionId()
        {
            var rand = new Random();
            var sessionId = 0;

            do
            {
                sessionId = rand.Next(int.MinValue, int.MaxValue);
            }
            while (this.sessionRepository.Get(sessionId) != null);

            return sessionId;
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

                        var sessionsToTerminate = this.sessionRepository.GetExpiredSessions();

                        foreach (var session in sessionsToTerminate)
                        {
                            await this.TerminateSession(session, SessionActivityType.TimeOut);
                            this.logger.LogInformation($"Successfully timed out session: [{session.SessionId}] for user: [{session.UserName}]");
                        }
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

        private void HandleSessionTermination(TerminateSessionNotification terminateSessionNotification)
        {
            this.slimLock.Wait();

            try
            {
                // Retrieve and remove the connections to the session we're closing
                var kvps = this.connectionSessionMap.Where(x => x.Value.SessionId == terminateSessionNotification.SessionId).ToArray();

                foreach (var kvp in kvps)
                {
                    this.connectionSessionMap.Remove(kvp.Key);
                }

                var connections = kvps.Select(x => x.Key).ToArray();

                this.hubConnectionService.TerminateConnections(terminateSessionNotification.Reason, connections);
            }
            finally
            {
                this.slimLock.Release();
            }
        }

        private void HandleOnBehalfChange(int sessionId)
        {
            this.slimLock.Wait();

            try
            {
                var session = this.sessionRepository.Get(sessionId);

                if (session == null)
                {
                    this.logger.LogWarning($"Session with ID [{sessionId}] was not found while trying to update on behalf state");
                    return;
                }

                var kvps = this.connectionSessionMap.Where(x => x.Value.SessionId == sessionId).ToArray();

                foreach (var kvp in kvps)
                {
                    this.connectionSessionMap[kvp.Key] = session;
                }
            }
            finally
            {
                this.slimLock.Release();
            }
        }
    }
}
