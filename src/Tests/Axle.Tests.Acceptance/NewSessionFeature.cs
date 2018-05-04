namespace Axle.Tests.Acceptance
{
    using System.Threading.Tasks;
    using Axle.Tests.Acceptance.Support;
    using FluentAssertions;
    using Xbehave;

    public class NewSessionFeature : IntegrationTest
    {
        private const string AxleUserId = "abc";

        public NewSessionFeature(AxleFixture axleFixture)
            : base(axleFixture)
        {
        }

        [Scenario]
        public void OpeningANewSessionShouldOpenASignalRConnection()
        {
            var signalRClient = new SignalRClient(this.AxleUrl);

            "Given a SignalR client"
                .x(async () =>
                {
                    await signalRClient.StartConnection();
                });

            "When I open a session in Axle with the axle-test user ID"
                .x(async () =>
                {
                    await signalRClient.StartSession(AxleUserId);
                });

            "Then a connection with the SignalR hub is open"
                .x(async () =>
                {
                    await Task.Delay(100);
                    signalRClient.IsConnected.Should().BeTrue();
                })
                .Teardown(async () =>
                {
                    await signalRClient.Teardown();
                });
        }

        [Scenario]
        public void OpeningANewSessionShouldCloseAnOlderActiveSession()
        {
            var signalRClient = new SignalRClient(this.AxleUrl);
            var signalRClient2 = new SignalRClient(this.AxleUrl);

            "Given an open session with the axle-test user ID"
                .x(async () =>
                {
                    await signalRClient.StartConnection();
                    await signalRClient.StartSession(AxleUserId);
                });

            "When I try to open another session with the same user ID"
                .x(async () =>
                {
                    await signalRClient2.StartConnection();
                    await signalRClient2.StartSession(AxleUserId);
                });

            "Then the first session gets terminated"
                .x(async () =>
                {
                    await Task.Delay(100);
                    signalRClient.IsConnected.Should().BeFalse();
                    signalRClient2.IsConnected.Should().BeTrue();
                })
                .Teardown(async () =>
                {
                    await signalRClient.Teardown();
                    await signalRClient2.Teardown();
                });
        }
    }
}
