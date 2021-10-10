using System.Text.Json;

namespace Producer.Sdk;

internal sealed class ProducerClient : IProducerClient
{
    private readonly HttpClient _httpClient;

    public ProducerClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<Response<DateResponse, Error>> GetDate(string date)
    {
        var response = await _httpClient.GetAsync($"/api/provider?date={date}");

        if (response.IsSuccessStatusCode)
        {
            var stream = await response.Content.ReadAsStreamAsync();
            var dateResponse = await JsonSerializer.DeserializeAsync<DateResponse>(stream);
            return Response<DateResponse, Error>.Success(dateResponse!); // todo: deal with this
        }

        var errorStream = await response.Content.ReadAsStreamAsync();
        var errorResponse = await JsonSerializer.DeserializeAsync<Error>(errorStream);
        return Response<DateResponse, Error>.Error(errorResponse);
    }
}
