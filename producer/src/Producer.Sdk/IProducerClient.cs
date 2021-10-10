namespace Producer.Sdk;

public interface IProducerClient
{
    Task<Response<DateResponse, Error>> GetDate(string date); 
}

public record DateResponse(string date);
public record Error(string message);
