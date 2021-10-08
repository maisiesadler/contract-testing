using System.Collections.Generic;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using PactNet.Mocks.MockHttpService;

namespace Consumer.Tests;

public class TestFixture : WebApplicationFactory<Program>
{
    private readonly ProviderMock _providerMock = new();
    public IMockProviderService MockProviderService => _providerMock.MockProviderService;

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        base.ConfigureWebHost(builder);

        builder
            .ConfigureAppConfiguration((context, config) =>
            {
                config.Sources.Clear();
                config.AddInMemoryCollection(GetConfigurationOverrides());
            });
    }

    private Dictionary<string, string> GetConfigurationOverrides()
    {
        return new()
        {
            { "AppSettings:Provider:BaseUri", _providerMock.MockProviderServiceBaseUri },
        };
    }
}
