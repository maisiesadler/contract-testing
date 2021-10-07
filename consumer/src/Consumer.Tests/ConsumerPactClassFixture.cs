using System;
using Xunit;
using PactNet;
using PactNet.Mocks.MockHttpService;
using System.IO;

namespace Consumer.Tests;

public class ConsumerPactClassFixture : IDisposable
{
    public IPactBuilder PactBuilder { get; private set; }
    public IMockProviderService MockProviderService { get; private set; }

    public int MockServerPort { get { return 9222; } }
    public string MockProviderServiceBaseUri { get { return String.Format("http://localhost:{0}", MockServerPort); } }

    public ConsumerPactClassFixture()
    {
        var dir = "../../../../../"; // from bin to consumer root dir
        // Using Spec version 2.0.0 more details at https://goo.gl/UrBSRc
        var pactConfig = new PactConfig
        {
            SpecificationVersion = "2.0.0",
            PactDir = $"{dir}pacts",
            LogDir = $"{dir}pact_logs",
        };

        System.Console.WriteLine($"{dir}pacts");

        PactBuilder = new PactBuilder(pactConfig);

        PactBuilder.ServiceConsumer("Consumer")
                   .HasPactWith("Provider");

        MockProviderService = PactBuilder.MockService(MockServerPort, useRemoteMockService: false);
    }

    #region IDisposable Support
    private bool disposedValue = false; // To detect redundant calls

    protected virtual void Dispose(bool disposing)
    {
        if (!disposedValue)
        {
            if (disposing)
            {
                // This will save the pact file once finished.
                PactBuilder.Build();
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