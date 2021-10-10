using System.Threading.Tasks;
using Xunit;

namespace Producer.ContractTests;

public class SdkToContractTests : IClassFixture<TestFixture>
{
    private readonly TestFixture _testFixture;

    public SdkToContractTests(TestFixture testFixture)
    {
        _testFixture = testFixture;
    }

    [Fact]
    public async Task CanUseSdkToCallContracts()
    {
        // Arrange
        var client = _testFixture.GetProducerClient();

        // Act
        var response = await client.GetDate("lolz");

        // Assert
        Assert.False(response.IsSuccess);
        Assert.Equal("hello", response.ErrorValue?.message);
    }
}
