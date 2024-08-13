namespace MaisQ1Dev.Libs.Domain;

public class PagedResult<TValue>
{
    public PagedResult(
        long pageNumber,
        long pageSize,
        long totalRecords,
        TValue item)
    {
        PageNumber = pageNumber;
        PageSize = pageSize;
        TotalRecords = totalRecords;
        Item = item;
    }

    public long PageNumber { get; private set; }
    public long PageSize { get; private set; }
    public long TotalRecords { get; private set; }
    public TValue Item { get; private set; }
}