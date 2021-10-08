using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using PactNet.Mocks.MockHttpService;
using PactNet.Mocks.MockHttpService.Models;

namespace Consumer.Tests;

public class ConsumerPactTests : IClassFixture<TestFixture>
{
    private readonly TestFixture _fixture;
    private readonly IMockProviderService _mockProviderService;

    public ConsumerPactTests(TestFixture fixture)
    {
        _mockProviderService = fixture.MockProviderService;
        _mockProviderService.ClearInteractions();
        _fixture = fixture;
    }

    [Fact]
    public async Task ItHandlesInvalidDateParam()
    {
        // Arange
        var invalidRequestMessage = "validDateTime is not a date or time";
        _mockProviderService
           .Given("There is data")
            .UponReceiving("A invalid GET request for Date Validation with invalid date parameter")
            .With(new ProviderServiceRequest
            {
                Method = HttpVerb.Get,
                Path = "/api/provider",
                Query = "validDateTime=lolz"
            })
            .WillRespondWith(new ProviderServiceResponse
            {
                Status = 400,
                Headers = new Dictionary<string, object>
                {
                { "Content-Type", "application/json; charset=utf-8" }
                },
                Body = new
                {
                    message = invalidRequestMessage
                }
            });
        var client = _fixture.CreateClient();

        // Act
        var response = await client.GetAsync("/");
        var resultBodyText = await response.Content.ReadAsStringAsync();

        // Assert
        Assert.Contains(invalidRequestMessage, resultBodyText);
    }
}
