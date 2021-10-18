using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace Producer.Tests;

public class ProviderApi : IDisposable
{
    private readonly WebApplication _app;
    private readonly Task _appTask;
    public Uri BaseAddress { get; }

    public ProviderApi(string baseAddress)
    {
        var builder = WebApplication.CreateBuilder();
        _app = builder.Build();
        _app.MapEndpoints();

        _appTask = _app.RunAsync(baseAddress);
        BaseAddress = new Uri(baseAddress);

        Console.WriteLine($"Provider API running at {BaseAddress}");
    }

    public void Dispose()
    {
        _app.StopAsync().Wait();
        _appTask.Wait();
        GC.SuppressFinalize(this);
    }

    private Dictionary<string, string> GetInMemoryConfiguration()
    {
        return new()
        {
        };
    }
}