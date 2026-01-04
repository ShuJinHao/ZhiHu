using CommunityToolkit.Aspire.Hosting.Dapr;
using Projects;

namespace Zhihu.AppHost;

public static class QuestionServiceBuilder
{
    private const string MasterDb = nameof(MasterDb);
    private const string SlaveDb = nameof(SlaveDb);

    public static void AddQuestionService(this IDistributedApplicationBuilder builder,
        IResourceBuilder<MySqlServerResource> mysql,
        IResourceBuilder<RedisResource> redis,
        IResourceBuilder<RabbitMQServerResource> rabbitmq,
        DaprSidecarOptions? daprSidecarOptions = null)
    {
        var db = mysql.AddDatabase("QuestionDb");

        var migration = builder.AddProject<Zhihu_QuestionService_MigrationWorker>("QuestionService-MigrationWorker")
            .WithReference(db, MasterDb)
            .WaitFor(mysql);

        builder.AddProject<Zhihu_QuestionService_HttpApi>("QuestionService-HttpApi")
            .WithReference(db, MasterDb)
            .WithReference(db, SlaveDb)
            .WaitFor(mysql)
            .WithReference(redis)
            .WaitFor(redis)
            .WithDaprSidecar(daprSidecarOptions)
            .WaitFor(rabbitmq)
            .WaitForCompletion(migration);
    }
}