using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;
using PactNet.Mocks.MockHttpService;
using PactNet.Mocks.MockHttpService.Models;
using Consumer;
using System.IO;

namespace Consumer.Tests;

public class ConsumerPactTests : IClassFixture<ConsumerPactClassFixture>
{
    private IMockProviderService _mockProviderService;
    private string _mockProviderServiceBaseUri;

    public ConsumerPactTests(ConsumerPactClassFixture fixture)
    {
        _mockProviderService = fixture.MockProviderService;
        _mockProviderService.ClearInteractions(); //NOTE: Clears any previously registered interactions before the test is run
        _mockProviderServiceBaseUri = fixture.MockProviderServiceBaseUri;
    }

    [Fact]
    public void ItHandlesInvalidDateParam()
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

        // Act
        var result = ConsumerApiClient.ValidateDateTimeUsingProviderApi("lolz", _mockProviderServiceBaseUri).GetAwaiter().GetResult();
        var resultBodyText = result.Content.ReadAsStringAsync().GetAwaiter().GetResult();

        // Assert
        Assert.Contains(invalidRequestMessage, resultBodyText);
    }

    public static class ConsumerApiClient
    {
        static public async Task<HttpResponseMessage> ValidateDateTimeUsingProviderApi(string dateTimeToValidate, string baseUri)
        {
            using (var client = new HttpClient { BaseAddress = new Uri(baseUri) })
            {
                try
                {
                    var response = await client.GetAsync($"/api/provider?validDateTime={dateTimeToValidate}");
                    return response;
                }
                catch (System.Exception ex)
                {
                    throw new Exception("There was a problem connecting to Provider API.", ex);
                }
            }
        }
    }
}
