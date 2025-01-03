namespace CharityHub.Core.Contract;
public class PagedData<T>
{
    public List<T> Items { get; set; }
    public int TotalCount { get; set; }
}
