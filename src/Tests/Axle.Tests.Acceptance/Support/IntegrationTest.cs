// (c) Lykke Corporation 2019 - All rights reserved. No copying, adaptation, decompiling, distribution or any other form of use permitted.

namespace Axle.Tests.Acceptance.Support
{
    using System;
    using Xunit;

    [Collection("Axle")]
    public class IntegrationTest
    {
        private readonly AxleFixture axleFixture;

        public IntegrationTest(AxleFixture axleFixture)
        {
            this.axleFixture = axleFixture;
        }

        protected Uri AxleUrl => this.axleFixture.AxleUrl;
    }
}
