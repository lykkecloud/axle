namespace Axle.Tests.Acceptance.Support
{
    using Xunit;

    [CollectionDefinition("Axle2")]
    public class Axle2Collection : ICollectionFixture<AxleFixture>
    {
        // This class has no code, and is never created. Its purpose is simply
        // to be the place to apply [CollectionDefinition] and all the
        // ICollectionFixture<> interfaces.
    }
}
