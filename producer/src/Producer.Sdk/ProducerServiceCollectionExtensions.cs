namespace Producer.Sdk;

public static class ProducerServiceCollectionExtensions
{
    public static IHttpClientBuilder AddProducerClient(this IServiceCollection services)
    {
        services.AddTransient<IProducerClient, ProducerClient>();
        return services.AddHttpClient<IProducerClient, ProducerClient>();
    }
}
