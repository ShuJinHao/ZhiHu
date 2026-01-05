using Zhihu.SharedKernel.Paging;
using Zhihu.SharedKernel.Search;

namespace Zhihu.SearchService.Infrastructure;

public interface ISearchService
{
    Task<SearchResult<TDoc>> ShouldMatchQuery<TDoc>(string indices, string[] fields, string query,
        Pagination pagination) where TDoc : class;

    Task<SearchResult<TDoc>> MatchQuery<TDoc>(string indices, string field, string query,
        Pagination pagination) where TDoc : class;
}
