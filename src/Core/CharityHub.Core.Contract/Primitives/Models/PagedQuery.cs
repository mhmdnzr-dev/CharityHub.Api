namespace CharityHub.Core.Contract.Primitives.Models;

using CharityHub.Core.Contract.Primitives.Handlers;

public abstract class PagedQuery<TResponse> : IQuery<PagedData<TResponse>>
{
    public int Page { get; set; } = 1;  // Default to first page
    public int PageSize { get; set; } = 10; // Default page size
}
