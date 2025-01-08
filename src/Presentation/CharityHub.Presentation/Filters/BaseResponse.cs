namespace CharityHub.Presentation.Filters;
internal sealed class BaseResponse<T>
{
    public bool Success { get; set; }
    public T Data { get; set; }
    public string ErrorMessage { get; set; }
    public int StatusCode { get; set; }
}
