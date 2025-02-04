namespace CharityHub.Core.Contract.Primitives.Models;
public class Result<T>
{
    public bool IsSuccess { get; }
    public T? Data { get; }
    public string? ErrorMessage { get; }
    public Exception? Exception { get; }

    private Result(bool isSuccess, T? data = default, string? errorMessage = null, Exception? exception = null)
    {
        IsSuccess = isSuccess;
        Data = data;
        ErrorMessage = errorMessage;
        Exception = exception;
    }

    public static Result<T> Success(T data) => new Result<T>(true, data);

    public static Result<T> Failure(string errorMessage, Exception? exception = null)
        => new Result<T>(false, default, errorMessage, exception);

    public override string ToString()
    {
        return IsSuccess
            ? $"Success: {Data}"
            : $"Failure: {ErrorMessage}, Exception: {Exception?.Message}";
    }
}
