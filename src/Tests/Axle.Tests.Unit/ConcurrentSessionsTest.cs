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
            IHubContext<SessionHub> hubContext = null;
            ISessionRepository sessionRepository = null;
            IReadOnlyRepository<string, HubConnectionContext> connectionRepository = null;
            Thread[] threads = null;
            SessionHubMethods<SessionHub> hubMethods = null;
            ConcurrentQueue<Exception> exceptions = null;

            "Given Axle SignalR hub"
                .x(() =>
                {
                    exceptions = new ConcurrentQueue<Exception>();
                    threads = new Thread[numberOfSessions];

                    hubContext = A.Fake<IHubContext<SessionHub>>();
                    sessionRepository = new InMemorySessionRepository();

                    connectionRepository = A.Fake<IReadOnlyRepository<string, HubConnectionContext>>();
                    A.CallTo(() => connectionRepository.Get(A<string>.Ignored)).Returns(A.Fake<HubConnectionContext>());

                    hubMethods = new SessionHubMethods<SessionHub>(hubContext, sessionRepository, connectionRepository);
                });

            "When I open multiple sessions with the same user ID"
                .x(() =>
                {
                    for (int i = 0; i < numberOfSessions; i++)
                    {
                        threads[i] = new Thread(() => SafeExecute(() => this.StartSession(hubMethods), exceptions));
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
                    activeSessionsOfUser.Count().Should().Be(1);
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

        private void StartSession(SessionHubMethods<SessionHub> hubMethods, string sessionId = null)
        {
            sessionId = sessionId ?? Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture);

            hubMethods.StartSession(Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture), UserId, sessionId);
        }
    }
}
