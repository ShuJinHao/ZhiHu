using Elastic.Clients.Elasticsearch;
using Elastic.Clients.Elasticsearch.Core.Search;
using Elastic.Clients.Elasticsearch.QueryDsl;
using Zhihu.SharedKernel.Paging;
using Zhihu.SharedKernel.Search;

namespace Zhihu.SearchService.Infrastructure;

public class ElasticSearchService(ElasticsearchClient client) : ISearchService
{
    public async Task<SearchResult<TDoc>> ShouldMatchQuery<TDoc>(string indices, string[] fields, string query,
        Pagination pagination)
        where TDoc : class
    {
        var request = new SearchRequest(indices)
        {
            Query = new BoolQuery
            {
                Should = fields
                    .Select(field => Query.Match(new MatchQuery(new Field(field)) { Query = query }))
                    .ToList()
            },
            Highlight = new Highlight
            {
                Fields = fields.ToDictionary(
                    field => new Field(field),
                    _ => new HighlightField
                    {
                        FragmentSize = 50,
                        NumberOfFragments = 1,
                        NoMatchSize = 50
                    })
            },
            From = (pagination.PageNumber - 1) * pagination.PageSize,
            Size = pagination.PageSize
        };

        var response = await client.SearchAsync<TDoc>(request);

        var items = new List<SearchResultItem<TDoc>>();
        items.AddRange(response.Hits.Select(hit => new SearchResultItem<TDoc>
        {
            Index = hit.Index,
            Score = hit.Score,
            Source = hit.Source,
            Highlight = hit.Highlight
        }));

        return new SearchResult<TDoc>(items, response.Total, pagination);
    }

    public async Task<SearchResult<TDoc>> MatchQuery<TDoc>(string indices, string field, string query,
        Pagination pagination) where TDoc : class
    {
        var request = new SearchRequest(indices)
        {
            Query = new MatchQuery(new Field(field)) { Query = query },
            Highlight = new Highlight
            {
                Fields = new Dictionary<Field, HighlightField>
                {
                    { new Field(field), new HighlightField() }
                }
            },
            From = (pagination.PageNumber - 1) * pagination.PageSize,
            Size = pagination.PageSize
        };

        var response = await client.SearchAsync<TDoc>(request);

        var items = new List<SearchResultItem<TDoc>>();
        items.AddRange(response.Hits.Select(hit => new SearchResultItem<TDoc>
        {
            Index = hit.Index,
            Score = hit.Score,
            Source = hit.Source,
            Highlight = hit.Highlight
        }));

        return new SearchResult<TDoc>(items, response.Total, pagination);
    }
}