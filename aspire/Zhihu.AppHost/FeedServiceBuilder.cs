using CommunityToolkit.Aspire.Hosting.Dapr;
using Projects;

namespace Zhihu.AppHost;

public static class FeedServiceBuilder
{
    private const string MasterDb = nameof(MasterDb);
    private const string SlaveDb = nameof(SlaveDb);

    public static void AddFeedService(this IDistributedApplicationBuilder builder,
        IResourceBuilder<MySqlServerResource> mysql,
        IResourceBuilder<RabbitMQServerResource> rabbitmq,
        DaprSidecarOptions? daprSidecarOptions = null)
    {
        var db = mysql.AddDatabase("zhihu-feed");

        var appId = builder.Configuration["AppId:FeedService"];
        ArgumentNullException.ThrowIfNull(appId);

        var migration = builder.AddProject<Zhihu_FeedService_MigrationWorker>("FeedService-MigrationWorker")
            .WithReference(db, MasterDb)
            .WaitFor(mysql);

        builder.AddProject<Zhihu_FeedService_HttpApi>(appId)
            .WithReference(db, MasterDb)
            .WithReference(db, SlaveDb)
            .WaitFor(mysql)
            .WithDaprSidecar(daprSidecarOptions)
            .WaitFor(rabbitmq)
            .WaitForCompletion(migration);
    }
}