using Microsoft.EntityFrameworkCore;

namespace API.Core;

public class PagedList<T>
{
    public int CurrentPage { get; set; }
    public int TotalPages { get; set; }
    public int TotalItems { get; set; }
    public int PageSize { get; set; }
    public List<T> Items { get; set; }

    public static async Task<PagedList<T>> CreateAsync(IQueryable<T> source, PagingParams pagingParams)
    {
        var count = await source.CountAsync();
        var totalPages = (int)Math.Ceiling(count / (double)pagingParams.PageSize);

        if (pagingParams.Page > totalPages)
        {
            return new PagedList<T>
            {
                CurrentPage = pagingParams.Page,
                PageSize = pagingParams.PageSize,
                TotalItems = count,
                TotalPages = totalPages,
                Items =  new List<T>(),
            };
        }

        var items = await source
            .Skip((pagingParams.Page - 1) * pagingParams.PageSize)
            .Take(pagingParams.PageSize)
            .ToListAsync();

        return new PagedList<T>
        {
            CurrentPage = pagingParams.Page,
            PageSize = pagingParams.PageSize,
            TotalItems = count,
            TotalPages = totalPages,
            Items = items,
        };
    }
}