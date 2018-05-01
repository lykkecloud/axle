namespace Axle.Tests.Acceptance
{
    using System;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Axle.Tests.Acceptance.Support;
    using FluentAssertions;
    using Microsoft.AspNetCore.SignalR.Client;
    using Xbehave;
    using Xunit;

    [Collection("Axle2")]
    public class ConcurrentSessionsFeature : IntegrationTest
    {
        public ConcurrentSessionsFeature(AxleFixture axleFixture)
            : base(axleFixture)
        {
        }

        [Scenario(Skip = "To be replaced by a unit test")]
        public void OpeningMultipleSessionsShouldKeepOnlyOneActiveSession()
        {
            var numberOfSessions = 3;
            var signalRClients = Enumerable.Range(0, numberOfSessions).Select(x => new SignalRClient(this.AxleUrl)).ToArray();
            var threads = Enumerable.Range(0, numberOfSessions).Select(x => new Thread(this.StartSessionSynchronously)).ToArray();

            "Given multiple connections to Axle"
                .x(async () =>
                {
                    foreach (var client in signalRClients)
                    {
                        await client.StartConnection();
                    }
                });

            "When I try to open multiple sessions with the same user ID at once"
                .x(() =>
                {
                    for (int i = 0; i < numberOfSessions; i++)
                    {
                        threads[i].Start(signalRClients[i]);
                    }
                });

            "Then only one session remains connected"
                .x(async () =>
                {
                    await Task.Delay(100);
                    var connected = 0;
                    var disconnected = 0;

                    foreach (var client in signalRClients)
                    {
                        if (client.IsConnected)
                        {
                            connected++;
                        }
                        else
                        {
                            disconnected++;
                        }
                    }

                    connected.Should().Be(1);
                    disconnected.Should().Be(numberOfSessions - 1);
                })
                .Teardown(async () =>
                {
                    foreach (var thread in threads)
                    {
                        thread.Join();
                    }

                    foreach (var client in signalRClients)
                    {
                        await client.Teardown();
                    }
                });
        }

        private void StartSessionSynchronously(object obj)
        {
            // TODO (Marta): This is quite horrendous, I should implement a unit test for the concurrency stuff instead.
            var client = (SignalRClient)obj;
            try
            {
                client.StartSession("abc").Wait();
            }
            catch (AggregateException aex)
            {
                if (aex.InnerException.GetType() != typeof(HubException))
                {
                    throw;
                }
            }
        }
    }
}
