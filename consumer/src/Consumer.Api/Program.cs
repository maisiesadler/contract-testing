using Consumer.Api;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddHttpClient<GetDateQuery>()
    .ConfigureHttpClient((sp, client) =>
    {
        var configuration = sp.GetRequiredService<IConfiguration>();
        client.BaseAddress = new Uri(configuration["AppSettings:Provider:BaseUri"]);
    });

var app = builder.Build();

app.MapGet("/", async (GetDateQuery getDateQuery) =>
{
    var r = await getDateQuery.Execute();
    return r ? "ok" : "validDateTime is not a date or time";
});

app.Run();

public partial class Program
{
    // Expose the Program class for use with WebApplicationFactory<T>
}
