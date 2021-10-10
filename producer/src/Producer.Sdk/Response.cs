namespace Producer.Sdk;

public class Response<TModel, TError>
{
    public bool IsSuccess { get; }
    public TModel? Value { get; }
    public TError? ErrorValue { get; }

    private Response(bool isSuccess, TModel? value, TError? error)
    {
        IsSuccess = isSuccess;
        Value = value;
        ErrorValue = error;
    }

    public static Response<TModel, TError> Success(TModel model) => new Sdk.Response<TModel, TError>(true, model, default);
    public static Response<TModel, TError> Error(TError? error) => new Sdk.Response<TModel, TError>(false, default, error);
}
