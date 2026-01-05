using CommunityToolkit.Aspire.Hosting.Dapr;
using Projects;

namespace Zhihu.AppHost;

public static class SearchServiceBuilder
{
    public static void AddSearchService(this IDistributedApplicationBuilder builder,
        // 【修改点 1】参数类型改为 IResourceBuilder<ContainerResource>
        IResourceBuilder<ContainerResource> es,
        DaprSidecarOptions? daprSidecarOptions = null)
    {
        var appId = builder.Configuration["AppId:SearchService"];
        ArgumentNullException.ThrowIfNull(appId);

        builder.AddProject<Zhihu_SearchService_HttpApi>(appId)
            // 【修改点 2】手动注入连接字符串
            // 对应 SearchService 代码里的: configuration.GetConnectionString("es")
            // 因为我们在 AppHost 里把端口定死在 9200 了，这里直接写 localhost:9200 即可
            .WithEnvironment("ConnectionStrings__es", "http://localhost:9200")
            // 保持启动顺序：先启 ES，再启服务
            .WaitFor(es)
            .WithDaprSidecar(daprSidecarOptions);
    }
}