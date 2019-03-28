// (c) Lykke Corporation 2019 - All rights reserved. No copying, adaptation, decompiling, distribution or any other form of use permitted.

namespace Axle.Tests.Acceptance.Support
{
    using Xunit;

    [CollectionDefinition("Axle")]
    public class AxleCollection : ICollectionFixture<AxleFixture>
    {
        // This class has no code, and is never created. Its purpose is simply
        // to be the place to apply [CollectionDefinition] and all the
        // ICollectionFixture<> interfaces.
    }
}
