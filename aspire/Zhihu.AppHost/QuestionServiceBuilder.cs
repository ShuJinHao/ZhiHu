using CommunityToolkit.Aspire.Hosting.Dapr;
using Projects;

namespace Zhihu.AppHost;

public static class QuestionServiceBuilder
{
    private const string MasterDb = nameof(MasterDb);
    private const string SlaveDb = nameof(SlaveDb);

    public static void AddQuestionService(this IDistributedApplicationBuilder builder,
     IResourceBuilder<MySqlServerResource> mysql,
     // 【修改 1】这里必须改类型，因为现在传进来的是个纯容器
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
            // 【修改 2】纯容器没有魔法，WithReference 没法自动注入连接字符串
            // 我们直接告诉它连哪里 (对应 Program.cs 里的 6380 端口)
            // 注意：这里的 Key "ConnectionStrings__redis" 要和你 appsettings.json 或代码里读取的名字一致
            // 如果你的代码里写的是 AddRedisClient("cache1")，这里就写 "ConnectionStrings__cache1"
            .WithEnvironment("ConnectionStrings__redis", "localhost:6380,password=123456")
            .WaitFor(redis)
            .WithDaprSidecar(daprSidecarOptions)
            .WaitFor(rabbitmq)
            .WaitForCompletion(migration);
    }
}