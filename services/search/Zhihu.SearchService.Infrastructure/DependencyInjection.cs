using Elastic.Clients.Elasticsearch;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Zhihu.SearchService.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        var esConn = configuration.GetConnectionString("es");
        ArgumentNullException.ThrowIfNull(esConn);
        var settings = new ElasticsearchClientSettings(new Uri(esConn));
        
        services.AddSingleton(new ElasticsearchClient(settings));

        services.AddSingleton<ISearchService, ElasticSearchService>();

        return services;
    }
}