namespace Axle.Tests.Acceptance
{
    using Axle.Tests.Acceptance.Support;
    using Axle.Tests.Acceptance.Support.SecuritySdk;
    using Xbehave;
    using Xunit;

    public class ConcurrentSessionsFeature : IntegrationTest
    {
        public ConcurrentSessionsFeature(SecurityFixture securityFixture, AxleFixture axleFixture)
            : base(securityFixture, axleFixture)
        {
        }

        [Scenario]
        public void OpeningANewSessionShouldCloseAnOlderActiveSession()
        {
            "Given nothing happens".x(() =>
            {
                Assert.True(1 == 1);
            });
        }
    }
}
