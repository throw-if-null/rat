using Xunit;

namespace Rat.Api.Test
{
	[CollectionDefinition("Integration")]
    public class IntegrationTestsCollection : ICollectionFixture<RatFixture>
    {
    }
}
