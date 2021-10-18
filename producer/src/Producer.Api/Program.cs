var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapEndpoints();

app.Run();

public partial class Program
{
    // Expose the Program class for use with WebApplicationFactory<T>
}

public static class ServiceCollectionExtensions
{ }

public static class AppBuilderExtensions
{
    public static WebApplication MapEndpoints(this WebApplication app)
    {
        app.UseMiddleware<ActivityEnrichingMiddleware>();
        app.MapGet("/api/provider", () => Results.BadRequest(new { message = "hello" }));
        return app;
    }
}

public class ActivityEnrichingMiddleware
{
    private readonly RequestDelegate _next;

    public ActivityEnrichingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        System.Console.WriteLine("Hello " + context.Request.Method);
        System.Console.WriteLine("Hello " + context.Request.Path);

        await _next(context);
        System.Console.WriteLine("G'bye " + context.Response.StatusCode);
    }
}
