using System;
using PactNet;
using PactNet.Mocks.MockHttpService;

namespace Consumer.Tests;

public class ProviderMock : IDisposable
{
    private IPactBuilder _pactBuilder;
    private int _mockServerPort => 9222;

    public IMockProviderService MockProviderService { get; }
    public string MockProviderServiceBaseUri => string.Format("http://localhost:{0}", _mockServerPort);

    public ProviderMock()
    {
        var dir = "../../../../../../"; // from bin to root dir
        // Using Spec version 2.0.0 more details at https://goo.gl/UrBSRc
        var pactConfig = new PactConfig
        {
            SpecificationVersion = "2.0.0",
            PactDir = $"{dir}pacts",
            LogDir = $"{dir}pact_logs",
        };

        _pactBuilder = new PactBuilder(pactConfig);

        _pactBuilder.ServiceConsumer("Consumer")
                   .HasPactWith("Provider");

        MockProviderService = _pactBuilder.MockService(_mockServerPort);
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
                _pactBuilder.Build();
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
