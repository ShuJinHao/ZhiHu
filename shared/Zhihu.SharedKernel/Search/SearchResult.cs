using Zhihu.SharedKernel.Paging;

namespace Zhihu.SharedKernel.Search;

public class SearchResult<TDoc> : List<SearchResultItem<TDoc>> where TDoc : class
{
    public SearchResult(IEnumerable<SearchResultItem<TDoc>> items, long count, Pagination pagination)
    {
        MetaData = new PagedMetaData
        {
            TotalCount = count,
            PageSize = pagination.PageSize,
            CurrentPage = pagination.PageNumber,
            TotalPages = (int)Math.Ceiling(count / (double)pagination.PageSize)
        };

        AddRange(items);
    }

    public PagedMetaData MetaData { get; set; }
}
