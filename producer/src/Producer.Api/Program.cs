var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/api/provider", () => Results.BadRequest(new { message = "hello" }));

app.Run();
