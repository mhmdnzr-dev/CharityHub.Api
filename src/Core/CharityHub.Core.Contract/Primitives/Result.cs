namespace CharityHub.Core.Contract.Primitives;

using CharityHub.Core.Contract.Exceptions;

public class Result<T>
{
    public bool IsSuccess { get; }
    public T? Data { get; }
    public BaseException? Error { get; }

    private Result(bool isSuccess, T? data = default, BaseException? error = null)
    {
        IsSuccess = isSuccess;
        Data = data;
        Error = error;
    }

    public static Result<T> Success(T data) => new Result<T>(true, data);

    public static Result<T> Error(BaseException error) => new Result<T>(false, error: error);
}