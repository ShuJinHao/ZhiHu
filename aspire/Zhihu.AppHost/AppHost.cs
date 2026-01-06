using CommunityToolkit.Aspire.Hosting.Dapr;
using Zhihu.AppHost;

Environment.SetEnvironmentVariable("ASPIRE_ALLOW_UNSECURED_TRANSPORT", "true");
var builder = DistributedApplication.CreateBuilder(args);

var mysql = builder.AddMySql("mysql")
    .WithPhpMyAdmin()
    .WithLifetime(ContainerLifetime.Persistent);

// 【还原】定义独立的 Redis 容器，端口映射为 6380
var redis = builder.AddContainer("redis", "redis", "latest")
    .WithEndpoint(port: 6380, targetPort: 6379, name: "redis-port")
    .WithBindMount(@"D:\Docker\RedisData", "/data")
    .WithArgs("--appendonly", "yes", "--requirepass", "123456");

var username = builder.AddParameter("username", secret: true);
var password = builder.AddParameter("password", secret: true);
var rabbitmq = builder.AddRabbitMQ("rabbitmq", username, password, 5672)
    .WithManagementPlugin()
    .WithLifetime(ContainerLifetime.Persistent);

// ES 容器保持不变
var es = builder.AddContainer("elasticsearch", "elasticsearch", "8.11.1")
    .WithEndpoint(port: 9200, targetPort: 9200, name: "http")
    .WithEnvironment("discovery.type", "single-node")
    .WithEnvironment("xpack.security.enabled", "false")
    .WithBindMount(@"D:\Docker\ESData", "/usr/share/elasticsearch/data");

var daprSidecarOptions = new DaprSidecarOptions
{
    ResourcesPaths = ["./DaprComponents"]
};

builder.AddUserService(mysql, daprSidecarOptions);

// 【还原】把 redis 参数加回来
builder.AddQuestionService(mysql, redis, rabbitmq, daprSidecarOptions);

builder.AddSearchService(es, daprSidecarOptions);

// 【还原】把 redis 参数加回来
builder.AddHotService(redis, rabbitmq, daprSidecarOptions);

builder.AddFeedService(mysql, rabbitmq, daprSidecarOptions);

builder.Build().Run();