namespace Axle.Tests.Acceptance
{
    using System;
    using System.Collections.Concurrent;
    using System.Threading;
    using System.Threading.Tasks;
    using Axle.Tests.Acceptance.Support;
    using FluentAssertions;
    using Xbehave;
    using Xunit;

    [Collection("Axle2")]
    public class ConcurrentSessionsFeature : IntegrationTest
    {
        public ConcurrentSessionsFeature(AxleFixture axleFixture)
            : base(axleFixture)
        {
        }

        [Scenario]
        [Example(3)]
        public void OpeningMultipleSessionsShouldKeepOnlyOneActiveSession(int numberOfSessions)
        {
            SignalRClient[] signalRClients = null;
            Thread[] threads = null;

            ConcurrentQueue<Exception> exceptions = null;

            "Given multiple connections to Axle"
                .x(async () =>
                {
                    exceptions = new ConcurrentQueue<Exception>();
                    signalRClients = new SignalRClient[numberOfSessions];

                    for (int i = 0; i < numberOfSessions; i++)
                    {
                        signalRClients[i] = new SignalRClient(this.AxleUrl);
                    }

                    foreach (var client in signalRClients)
                    {
                        await client.StartConnection();
                    }
                });

            "When I try to open multiple sessions with the same user ID at once"
                .x(() =>
                {
                    threads = new Thread[numberOfSessions];

                    for (int i = 0; i < numberOfSessions; i++)
                    {
                        var client = signalRClients[i];
                        threads[i] = new Thread(() => SafeExecute(() => this.StartSessionSynchronously(client), exceptions));
                    }

                    for (int i = 0; i < numberOfSessions; i++)
                    {
                        threads[i].Start();
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

                    exceptions.Should().BeEmpty();
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

        private void StartSessionSynchronously(SignalRClient client)
        {
            client.StartSession("abc").Wait();
        }
    }
}
