namespace CharityHub.Core.Contract.Primitives.Handlers;

using Models;

public abstract class PagedQuery<TResponse> : IQuery<PagedData<TResponse>>
{
    public int Page { get; set; } = 1;  // Default to first page
    public int PageSize { get; set; } = 10; // Default page size
}
