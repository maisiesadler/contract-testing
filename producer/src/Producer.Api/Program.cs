var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/api/provider", () => Results.BadRequest(new { message = "validDateTime is not a date or time" }));

app.Run();
