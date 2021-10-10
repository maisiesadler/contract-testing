using System;
using Microsoft.Extensions.DependencyInjection;
using Producer.Contracts;
using Producer.Sdk;

namespace Producer.ContractTests;

public class TestFixture : IDisposable
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ProviderMock _providerMock;

    public TestFixture()
    {
        _providerMock = new ProviderMock();
        _providerMock.MockProviderService.BuildContract();

        var services = new ServiceCollection();
        services
            .AddProducerClient()
            .ConfigureHttpClient(client => client.BaseAddress = _providerMock.MockProviderServiceBaseUri); ;

        _serviceProvider = services.BuildServiceProvider();
    }

    public IProducerClient GetProducerClient() => _serviceProvider.GetRequiredService<IProducerClient>();

    public void Dispose()
    {
        _providerMock.Dispose();
    }
}
