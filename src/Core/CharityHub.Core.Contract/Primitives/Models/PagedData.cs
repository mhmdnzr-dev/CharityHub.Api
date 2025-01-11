namespace CharityHub.Core.Contract.Primitives.Models;
public class PagedData<T>(IEnumerable<T> items, int totalCount, int pageSize, int currentPage)
{
    public IEnumerable<T> Items { get; set; } = items;
    public int TotalCount { get; set; } = totalCount;
    public int PageSize { get; set; } = pageSize;
    public int CurrentPage { get; set; } = currentPage;
}