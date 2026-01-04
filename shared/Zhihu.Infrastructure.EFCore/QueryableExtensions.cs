using Microsoft.EntityFrameworkCore;
using Zhihu.SharedKernel.Paging;

namespace Zhihu.Infrastructure.EFCore;

public static class QueryableExtensions
{
    public static async Task<PagedList<T>?> ToPageListAsync<T>(this IQueryable<T> queryable, Pagination pagination) where T : class
    {
        var count = queryable.Count();
        var items = await queryable
            .Skip((pagination.PageNumber - 1) * pagination.PageSize)
            .Take(pagination.PageSize)
            .ToListAsync();
        return items.Count == 0 ? null : new PagedList<T>(items, count, pagination);
    }
}