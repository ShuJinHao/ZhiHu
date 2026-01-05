using Elastic.Clients.Elasticsearch;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Zhihu.SearchService.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        // 1. 获取连接字符串
        // 这里会拿到我们在 AppHost 里注入的 "http://localhost:9200"
        var esConn = configuration.GetConnectionString("es");
        ArgumentNullException.ThrowIfNull(esConn);

        // 2. 配置 ES 客户端
        var uri = new Uri(esConn);
        var settings = new ElasticsearchClientSettings(uri)
            // 【关键修改】开启调试模式，如果报错可以看到详细信息
            .DisableDirectStreaming()
            // 【关键修改】设置超时时间，防止 Docker 刚启动响应慢导致报错
            .RequestTimeout(TimeSpan.FromMinutes(2));

        // 如果你是用 https（虽然我们现在用的是 http），加上这句可以跳过证书检查：
        // .ServerCertificateValidationCallback((sender, certificate, chain, errors) => true);

        services.AddSingleton(new ElasticsearchClient(settings));

        services.AddSingleton<ISearchService, ElasticSearchService>();

        return services;
    }
}