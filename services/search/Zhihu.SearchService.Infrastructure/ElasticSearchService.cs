using Elastic.Clients.Elasticsearch;
using Elastic.Clients.Elasticsearch.Core.Search;
using Elastic.Clients.Elasticsearch.QueryDsl;
using Zhihu.SharedKernel.Paging;
using Zhihu.SharedKernel.Search;
using Zhihu.SharedKernel.Result; // 确保引用了Result相关的命名空间，如果你的项目结构不同请调整

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

        // 【新增修复】核心改动在这里：检查响应是否有效
        if (!response.IsValidResponse || response.Hits == null)
        {
            // 这里抛出详细错误，你就能看到是“端口被拒”还是“证书错误”了，而不是不明不白的 NullReference
            throw new Exception($"ES查询失败。原因: {response.DebugInformation}");
        }

        var items = new List<SearchResultItem<TDoc>>();
        // 只有确认 Hits 不为空，才能调用 Select
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

        // 【新增修复】同上，防止空指针
        if (!response.IsValidResponse || response.Hits == null)
        {
            throw new Exception($"ES查询失败。原因: {response.DebugInformation}");
        }

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