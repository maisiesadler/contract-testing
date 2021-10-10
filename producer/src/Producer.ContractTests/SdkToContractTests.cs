using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Producer.Contracts;
using Producer.Sdk;
using Xunit;

namespace Producer.ContractTests;

public class SdkToContractTests
{
    [Fact]
    public async Task CanUseSdkToCallContracts()
    {
        // Arrange
        var providerMock = new ProviderMock();
        providerMock.MockProviderService.BuildContract();

        var services = new ServiceCollection();
        services
            .AddProducerClient()
            .ConfigureHttpClient(client => client.BaseAddress = providerMock.MockProviderServiceBaseUri);;

        var sp = services.BuildServiceProvider();

        var client = sp.GetRequiredService<IProducerClient>();

        // Act
        var response = await client.GetDate("lolz");

        // Assert
        Assert.False(response.IsSuccess);
        Assert.Equal("hello", response.ErrorValue?.message);
    }
}
