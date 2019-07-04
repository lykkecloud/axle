// Copyright (c) 2019 Lykke Corp.
// See the LICENSE file in the project root for more information.

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
