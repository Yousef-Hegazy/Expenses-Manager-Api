namespace API.Core;

public class PagingParams
{
    private const int MaxSize = 50;
    private int _pageSize = 10;
    
    public int Page { get; set; } = 1;
    public int PageSize
    {
        get => _pageSize;
        set => _pageSize = (value > MaxSize) ? MaxSize : value;
    }
}