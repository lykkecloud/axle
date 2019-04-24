// (c) Lykke Corporation 2019 - All rights reserved. No copying, adaptation, decompiling, distribution or any other form of use permitted.

namespace Axle.Services
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Axle.Contracts;
    using Axle.Dto;
    using Axle.Extensions;
    using Axle.Persistence;
    using Microsoft.Extensions.Logging;

    public sealed class SessionService : ISessionService, IDisposable
    {
        private readonly ISessionRepository sessionRepository;
        private readonly ITokenRevocationService tokenRevocationService;
        private readonly INotificationService notificationService;
        private readonly IActivityService activityService;
        private readonly IAccountsService accountsService;
        private readonly ILogger<SessionService> logger;

        private readonly SemaphoreSlim slimLock = new SemaphoreSlim(1, 1);

        public SessionService(
            ISessionRepository sessionRepository,
            ITokenRevocationService tokenRevocationService,
            INotificationService notificationService,
            IActivityService activityService,
            IAccountsService accountsService,
            ILogger<SessionService> logger)
        {
            this.sessionRepository = sessionRepository;
            this.tokenRevocationService = tokenRevocationService;
            this.notificationService = notificationService;
            this.activityService = activityService;
            this.accountsService = accountsService;
            this.logger = logger;
        }

        public async Task<Session> BeginSession(
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
                    return userInfo;
                }

                var sessionId = this.GenerateSessionId();

                var newSession = new Session(userName, sessionId, accountId, accessToken, clientId, isSupportUser);

                this.sessionRepository.Add(newSession);

                if (!newSession.IsSupportUser)
                {
                    await this.activityService.PublishActivity(newSession, SessionActivityType.Login);
                }
                else
                {
                    await this.MakeAndPublishOnBehalfActivity(SessionActivityType.OnBehalfSupportConnected, newSession);
                }

                if (userInfo != null)
                {
                    await this.TerminateSession(userInfo, SessionActivityType.DifferentDeviceTermination);
                    this.logger.LogWarning(StatusCode.IF_ATH_502.ToMessage());
                }

                this.logger.LogWarning(StatusCode.IF_ATH_501.ToMessage());

                return newSession;
            }
            finally
            {
                this.slimLock.Release();
            }
        }

        public async Task<OnBehalfChangeResponse> UpdateOnBehalfState(int sessionId, string onBehalfAccount)
        {
            this.slimLock.Wait();

            try
            {
                var session = this.sessionRepository.Get(sessionId);

                if (session == null)
                {
                    return OnBehalfChangeResponse.Fail($"Unknown session ID [{sessionId}]");
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
                    await this.MakeAndPublishOnBehalfActivity(SessionActivityType.OnBehalfSupportDisconnected, session);
                }

                if (!string.IsNullOrEmpty(onBehalfAccount))
                {
                    await this.activityService.PublishActivity(new SessionActivity(SessionActivityType.OnBehalfSupportConnected, session.SessionId, onBehalfOwner, onBehalfAccount));
                }

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

            if (!userInfo.IsSupportUser)
            {
                await this.activityService.PublishActivity(userInfo, reason);
            }
            else if (!string.IsNullOrEmpty(userInfo.AccountId))
            {
                await this.MakeAndPublishOnBehalfActivity(SessionActivityType.OnBehalfSupportDisconnected, userInfo);
            }
        }

        public int GenerateSessionId()
        {
            var rand = new Random();
            int sessionId;

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

        private async Task MakeAndPublishOnBehalfActivity(SessionActivityType type, Session session)
        {
            var accountOwner = await this.accountsService.GetAccountOwnerUserName(session.AccountId);
            var sessionActivity = new SessionActivity(type, session.SessionId, accountOwner, session.AccountId);

            await this.activityService.PublishActivity(sessionActivity);
        }
    }
}
