// Copyright (c) Lykke Corp.
// See the LICENSE file in the project root for more information.

namespace Axle.Tests.Unit
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Threading;
    using Axle.Hubs;
    using Axle.Persistence;
    using Axle.Services;
    using FakeItEasy;
    using FluentAssertions;
    using Microsoft.AspNetCore.SignalR;
    using Xbehave;

    public class ConcurrentSessionsTest
    {
        private const string UserId = "abc";

        [Scenario]
        [Example(3)]
        public void OpeningMultipleSessionsShouldKeepOnlyOneActiveSession(int numberOfSessions)
        {
            ISessionRepository sessionRepository = null;
            Thread[] threads = null;
            SessionLifecycleService lifecycleService = null;
            ConcurrentQueue<Exception> exceptions = null;

            "Given Axle SignalR hub"
                .x(() =>
                {
                    exceptions = new ConcurrentQueue<Exception>();
                    threads = new Thread[numberOfSessions];

                    sessionRepository = new InMemorySessionRepository();

                    lifecycleService = new SessionLifecycleService(sessionRepository);
                });

            "When I open multiple sessions with the same user ID"
                .x(() =>
                {
                    for (int i = 0; i < numberOfSessions; i++)
                    {
                        threads[i] = new Thread(() => SafeExecute(() => this.StartSession(lifecycleService), exceptions));
                    }

                    for (int i = 0; i < numberOfSessions; i++)
                    {
                        threads[i].Start();
                    }
                });

            "Then only one session of that user remains"
                .x(() =>
                {
                    Thread.Sleep(500);
                    exceptions.Should().BeEmpty();

                    var activeSessionsOfUser = sessionRepository.GetByUser(UserId);
                    activeSessionsOfUser.Connections.Count().Should().Be(1);
                })
                .Teardown(() =>
                {
                    foreach (var thread in threads)
                    {
                        if (thread.IsAlive)
                        {
                            thread.Join();
                        }
                    }
                });
        }

        private static void SafeExecute(Action test, ConcurrentQueue<Exception> exceptions)
        {
            try
            {
                test.Invoke();
            }
            catch (Exception ex)
            {
                exceptions.Enqueue(ex);
            }
        }

        private void StartSession(SessionLifecycleService lifecycleService, string token = null)
        {
            token = token ?? Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture);

            var state = lifecycleService.OpenConnection(Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture), UserId, token);

            if (state != null)
            {
                foreach (var connection in state.Connections.ToList())
                {
                    lifecycleService.CloseConnection(connection);
                }
            }
        }
    }
}
