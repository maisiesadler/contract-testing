using PactNet.Mocks.MockHttpService;
using PactNet.Mocks.MockHttpService.Models;

namespace Producer.Contracts;

public static class ProducerContractExtensions 
{
    public static void BuildContract(this IMockProviderService mockProviderService)
    {
        mockProviderService
            .Given("There is data")
            .UponReceiving("A invalid GET request for Date Validation with invalid date parameter")
            .With(new ProviderServiceRequest
            {
                Method = HttpVerb.Get,
                Path = "/api/provider",
                Query = "date=lolz"
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
                    message = "hello"
                }
            });
    }
}
