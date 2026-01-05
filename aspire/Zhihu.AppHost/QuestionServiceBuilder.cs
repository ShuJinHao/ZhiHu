using CommunityToolkit.Aspire.Hosting.Dapr;
using Projects;

namespace Zhihu.AppHost;

public static class QuestionServiceBuilder
{
    private const string MasterDb = nameof(MasterDb);
    private const string SlaveDb = nameof(SlaveDb);

    public static void AddQuestionService(this IDistributedApplicationBuilder builder,
     IResourceBuilder<MySqlServerResource> mysql,
     // 【还原】加回参数
     IResourceBuilder<ContainerResource> redis,
     IResourceBuilder<RabbitMQServerResource> rabbitmq,
     DaprSidecarOptions? daprSidecarOptions = null)
    {
        var appId = builder.Configuration["AppId:QuestionService"];
        ArgumentNullException.ThrowIfNull(appId);

        var db = mysql.AddDatabase("zhihu-question");

        var migration = builder.AddProject<Zhihu_QuestionService_MigrationWorker>("QuestionService-MigrationWorker")
            .WithReference(db, MasterDb)
            .WaitFor(mysql);

        builder.AddProject<Zhihu_QuestionService_HttpApi>(appId)
            .WithReference(db, MasterDb)
            .WithReference(db, SlaveDb)
            .WaitFor(mysql)
            // 【还原】注入连接字符串，指向 6380
            // 确保这里是 6380，密码 123456
            .WithEnvironment("ConnectionStrings__redis", "localhost:6380,password=123456")
            .WaitFor(redis) // 等待 Redis 容器启动
            .WithDaprSidecar(daprSidecarOptions)
            .WaitFor(rabbitmq)
            .WaitForCompletion(migration);
    }
}