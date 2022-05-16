namespace Catalog_API.Pagination;

public abstract class QueryStringParameters
{
    public int PageNumber { get; set; } = 1;
    private int _pageSize;
    private int _maxPageSize = 50;

    public int PageSize
    {
        get { return _pageSize; }

        set { _pageSize = (value > _maxPageSize) ? _maxPageSize : (value < 0) ? 0 : value; }
    }
}
