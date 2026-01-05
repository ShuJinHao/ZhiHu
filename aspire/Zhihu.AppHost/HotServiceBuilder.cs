using CommunityToolkit.Aspire.Hosting.Dapr;
using Projects;

namespace Zhihu.AppHost;

public static class HotServiceBuilder
{
    public static void AddHotService(this IDistributedApplicationBuilder builder,
        // 【还原】加回参数
        IResourceBuilder<ContainerResource> redis,
        IResourceBuilder<RabbitMQServerResource> rabbitmq,
        DaprSidecarOptions? daprSidecarOptions = null)
    {
        var appId = builder.Configuration["AppId:HotService"];
        ArgumentNullException.ThrowIfNull(appId);

        builder.AddProject<Zhihu_HotService_HttpApi>(appId)
            // 【还原】注入连接字符串，指向 6380
            .WithEnvironment("ConnectionStrings__redis", "localhost:6380,password=123456")
            .WaitFor(redis)
            .WaitFor(rabbitmq)
            .WithDaprSidecar(daprSidecarOptions);
    }
}