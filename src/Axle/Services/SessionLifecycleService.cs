// Copyright (c) Lykke Corp.
// See the LICENSE file in the project root for more information.

namespace Axle.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Axle.Persistence;
    using Serilog;
    using StackExchange.Redis;

    public class SessionLifecycleService : ISessionLifecycleService
    {
        private readonly ISessionRepository sessionRepository;
        private readonly ITokenRevocationService tokenRevocationService;
        private readonly INotificationService notificationService;
        private readonly TimeSpan sessionTimeout;

        private readonly HashSet<Action<IEnumerable<string>>> closeConnectionCallbacks = new HashSet<Action<IEnumerable<string>>>();
        private readonly Dictionary<string, Session> connectionSessionMap = new Dictionary<string, Session>();
        private readonly object syncObj = new object();

        public SessionLifecycleService(
            ISessionRepository sessionRepository,
            ITokenRevocationService tokenRevocationService,
            INotificationService notificationService,
            TimeSpan sessionTimeout)
        {
            this.sessionRepository = sessionRepository;
            this.tokenRevocationService = tokenRevocationService;
            this.notificationService = notificationService;
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
            lock (this.syncObj)
            {
                this.connectionSessionMap.Remove(connectionId);
            }
        }

        public async Task OpenConnection(string connectionId, string userId, string clientId, string accessToken)
        {
            Session userInfo;

            lock (this.syncObj)
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
            }

            if (userInfo != null)
            {
                this.sessionRepository.Remove(userInfo.SessionId);
                await this.tokenRevocationService.RevokeAccessToken(userInfo.AccessToken, userInfo.ClientId);

                this.notificationService.PublishSessionTermination(userInfo.SessionId);
            }
        }

        private async void RefreshTimeouts()
        {
            while (true)
            {
                try
                {
                    await Task.Delay(this.sessionTimeout / 4);

                    lock (this.syncObj)
                    {
                        this.sessionRepository.RefreshSessionTimeouts(this.connectionSessionMap.Values);
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
            lock (this.syncObj)
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
        }
    }
}
