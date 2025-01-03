namespace CharityHub.Core.Contract;
public interface IPagedQuery<out TResponse> : IQuery<TResponse>
{
    int PageNumber { get; }
    int PageSize { get; }
}
