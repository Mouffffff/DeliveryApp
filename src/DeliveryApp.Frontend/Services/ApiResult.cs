namespace DeliveryApp.Frontend.Services;

public sealed class ApiResult<T>
{
    public bool IsSuccess { get; }
    public T? Value { get; }
    public string? Error { get; }
    public int? StatusCode { get; }

    private ApiResult(bool isSuccess, T? value, string? error, int? statusCode)
    {
        IsSuccess = isSuccess;
        Value = value;
        Error = error;
        StatusCode = statusCode;
    }

    public static ApiResult<T> Success(T value) => new(true, value, null, null);

    public static ApiResult<T> Failure(string error, int? statusCode = null) =>
        new(false, default, error, statusCode);
}
