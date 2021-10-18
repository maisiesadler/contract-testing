using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using PactNet;
using PactNet.Infrastructure.Outputters;
using Xunit;
using Xunit.Abstractions;

namespace Producer.Tests;

public class ProviderApiTests : IDisposable
{
    private string _providerUri { get; }
    private string _pactServiceUri { get; }
    private IWebHost _webHost { get; }
    private ITestOutputHelper _outputHelper { get; }

    public ProviderApiTests(ITestOutputHelper output)
    {
        _outputHelper = output;
        _providerUri = "http://localhost:9000";
        _pactServiceUri = "http://localhost:9001";

        _webHost = WebHost.CreateDefaultBuilder()
            // .UseKestrel(options => options.AllowSynchronousIO = false)
            .UseUrls(_pactServiceUri)
            .UseStartup<TestStartup>()
            .Build();

        _webHost.Start();
    }

    [Fact]
    public void EnsureProviderApiHonoursPactWithConsumer()
    {
        // Arrange
        var config = new PactVerifierConfig
        {
            // NOTE: We default to using a ConsoleOutput,
            // however xUnit 2 does not capture the console output,
            // so a custom outputter is required.
            Outputters = new List<IOutput>
            {
                new XUnitOutput(_outputHelper)
            },

            // Output verbose verification logs to the test output
            Verbose = true
        };

        var dir = "../../../../../../"; // from bin to root dir

        using var api = new ProviderApi("http://localhost:9000");

        // Act / Assert
        IPactVerifier pactVerifier = new PactVerifier(config);
        pactVerifier.ProviderState($"{_pactServiceUri}/provider-states")
            .ServiceProvider("Provider", api.BaseAddress.ToString())
            .HonoursPactWith("Consumer")
            .PactUri($"{dir}pacts/consumer-provider.json")
            .Verify();
    }

    // [Fact]
    // public void EnsureProviderApiHonoursPactWithConsumer_TestFixture()
    // {
    //     // Arrange
    //     var config = new PactVerifierConfig
    //     {
    //         // NOTE: We default to using a ConsoleOutput,
    //         // however xUnit 2 does not capture the console output,
    //         // so a custom outputter is required.
    //         Outputters = new List<IOutput>
    //         {
    //             new XUnitOutput(_outputHelper)
    //         },

    //         // Output verbose verification logs to the test output
    //         Verbose = true
    //     };

    //     using var service = new TestFixture();

    //     var client = service.CreateClient();

    //     var dir = "../../../../../../"; // from bin to root dir

    //     // Act / Assert
    //     IPactVerifier pactVerifier = new PactVerifier(config);
    //     pactVerifier.ProviderState($"{_pactServiceUri}/provider-states")
    //         .ServiceProvider("Provider", client.BaseAddress?.ToString())
    //         .HonoursPactWith("Consumer")
    //         .PactUri($"{dir}pacts/consumer-provider.json")
    //         .Verify();
    // }

    [Fact]
    public async Task CanCallTestFixtureWithHttpClient()
    {
        // Arrange
        using var api = new ProviderApi("http://localhost:9000");

        var client = new HttpClient { BaseAddress = api.BaseAddress };

        // Act
        var response = await client.GetAsync("/api/provider");

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    #region IDisposable Support
    private bool disposedValue = false; // To detect redundant calls

    protected virtual void Dispose(bool disposing)
    {
        if (!disposedValue)
        {
            if (disposing)
            {
                _webHost.StopAsync().GetAwaiter().GetResult();
                _webHost.Dispose();
            }

            disposedValue = true;
        }
    }

    // This code added to correctly implement the disposable pattern.
    public void Dispose()
    {
        // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        Dispose(true);
    }
    #endregion
}
