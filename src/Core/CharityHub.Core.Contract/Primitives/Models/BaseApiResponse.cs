namespace CharityHub.Core.Contract.Primitives.Models;

public class BaseApiResponse<T>
{
    public bool Success { get; set; }
    public T Data { get; set; }
    public string ErrorMessage { get; set; }
    public int StatusCode { get; set; }
}


