using CommunityToolkit.Aspire.Hosting.Dapr;
using Zhihu.AppHost;

Environment.SetEnvironmentVariable("ASPIRE_ALLOW_UNSECURED_TRANSPORT", "true");
var builder = DistributedApplication.CreateBuilder(args);

var mysql = builder.AddMySql("mysql")
    .WithPhpMyAdmin()
    .WithLifetime(ContainerLifetime.Persistent);

// 替换原来的 builder.AddRedis...
var redis = builder.AddContainer("redis", "redis", "latest")
    // 【端口】避开 Dapr 的 6379，强制用 6380
    .WithEndpoint(port: 6380, targetPort: 6379, name: "redis-port")
    // 【持久化】这里就是持久化！它替代了不稳定的 WithLifetime
    // 请确保 D:\Docker\RedisData 文件夹存在
    .WithBindMount(@"D:\Docker\RedisData", "/data")
    // 【密码】手动指定参数
    .WithArgs("--appendonly", "yes", "--requirepass", "123456");

var username = builder.AddParameter("username", secret: true);
var password = builder.AddParameter("password", secret: true);
var rabbitmq = builder.AddRabbitMQ("rabbitmq", username, password, 5672)
    .WithManagementPlugin()
    .WithLifetime(ContainerLifetime.Persistent);

var es = builder.AddConnectionString("es");

var daprSidecarOptions = new DaprSidecarOptions
{
    ResourcesPaths = ["./DaprComponents"]
};

builder.AddUserService(mysql, daprSidecarOptions);

builder.AddQuestionService(mysql, redis, rabbitmq, daprSidecarOptions);

builder.AddSearchService(es, daprSidecarOptions);

builder.Build().Run();