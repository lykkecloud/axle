// Copyright (c) Lykke Corp.
// See the LICENSE file in the project root for more information.

namespace Axle.Tests.Acceptance.Support
{
    using System.Net.Http;
    using Axle.Tests.Acceptance.Support.SecuritySdk;
    using Xunit;

    [Collection("Axle")]
    public class IntegrationTest
    {
        private readonly SecurityFixture securityFixture;
        private readonly AxleFixture axleFixture;

        public IntegrationTest(SecurityFixture securityFixture, AxleFixture axleFixture)
        {
            this.securityFixture = securityFixture;
            this.axleFixture = axleFixture;
        }

        protected string Authority => this.securityFixture.Authority;

        protected HttpMessageHandler Handler => this.securityFixture.Handler;
    }
}
