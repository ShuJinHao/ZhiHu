using CommunityToolkit.Aspire.Hosting.Dapr;
using Projects;

namespace Zhihu.AppHost;

public static class SearchServiceBuilder
{
    public static void AddSearchService(this IDistributedApplicationBuilder builder,
        IResourceBuilder<IResourceWithConnectionString> es,
        DaprSidecarOptions? daprSidecarOptions = null)
    {
        var appId = builder.Configuration["AppId:SearchService"];
        ArgumentNullException.ThrowIfNull(appId);

        builder.AddProject<Zhihu_SearchService_HttpApi>(appId)
            .WithReference(es)
            .WithDaprSidecar(daprSidecarOptions);
    }
}