namespace Axle.Tests.Acceptance.Support
{
    using Xunit;

    [CollectionDefinition("Axle collection")]
    public class AxleAcceptanceTestCollection : ICollectionFixture<AxleFixture>
    {
        // This class has no code, and is never created. Its purpose is simply
        // to be the place to apply [CollectionDefinition] and all the
        // ICollectionFixture<> interfaces.
    }
}
