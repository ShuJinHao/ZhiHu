using CommunityToolkit.Aspire.Hosting.Dapr;
using Projects;

// 必须引用，否则 EndpointProperty 找不到
using Aspire.Hosting.ApplicationModel;
using Aspire.Hosting;

namespace Zhihu.AppHost;

public static class AgentServiceBuilder
{
    private const string MasterDb = nameof(MasterDb);
    private const string SlaveDb = nameof(SlaveDb);

    public static void AddAgentService(this IDistributedApplicationBuilder builder,
        IResourceBuilder<MySqlServerResource> mysql,
        // ★★★ 核心修复：这里强制使用 RedisResource 类型，完美匹配 Program.cs ★★★
        // RedisResource 拥有 GetEndpoint 方法，所以下面不会再报错
        IResourceBuilder<RedisResource> redis,
        IResourceBuilder<RabbitMQServerResource> rabbitmq,
        DaprSidecarOptions? daprSidecarOptions = null)
    {
        var db = mysql.AddDatabase("zhihu-agent");

        var appId = builder.Configuration["AppId:AgentService"];
        ArgumentNullException.ThrowIfNull(appId);

        // 1. 获取 Endpoint (现在 redis 是 RedisResource，所以这行代码合法了)
        var redisEndpoint = redis.GetEndpoint("tcp");

        // 2. 拼接连接字符串 (Host:Port,password=123456)
        var redisConnStr = ReferenceExpression.Create($"{redisEndpoint.Property(EndpointProperty.Host)}:{redisEndpoint.Property(EndpointProperty.Port)},password=123456");

        var migration = builder.AddProject<Zhihu_AgentService_MigrationWorker>("AgentService-MigrationWorker")
            .WithReference(db, MasterDb)
            .WaitFor(mysql);

        builder.AddProject<Zhihu_AgentService_HttpApi>(appId)
            .WithReference(db, MasterDb)
            .WithReference(db, SlaveDb)
            .WaitFor(mysql)
            // 3. 手动注入环境变量，解决密码问题
            .WithEnvironment("ConnectionStrings__redis", redisConnStr)
            .WaitFor(redis)
            .WithDaprSidecar(daprSidecarOptions)
            .WaitFor(rabbitmq)
            .WaitForCompletion(migration);
    }
}