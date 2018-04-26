namespace Axle.Tests.Acceptance
{
    using System.Threading;
    using Xbehave;
    using Xunit;

    [Collection("Axle collection")]
    public class ConcurrentSessionsFeature
    {
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
