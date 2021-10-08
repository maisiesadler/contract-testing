namespace Consumer.Api;

public class GetDateQuery
{
    private readonly HttpClient _httpClient;

    public GetDateQuery(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<bool> Execute()
    {
        var dateTimeToValidate = "lolz";
        var response = await _httpClient.GetAsync($"/api/provider?validDateTime={dateTimeToValidate}");
        return response.IsSuccessStatusCode;
    }
}
