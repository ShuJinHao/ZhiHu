using CommunityToolkit.Aspire.Hosting.Dapr;
using Zhihu.AppHost;

// 确保引用了这个命名空间
using Aspire.Hosting.ApplicationModel;

var builder = DistributedApplication.CreateBuilder(args);

Environment.SetEnvironmentVariable("ASPIRE_ALLOW_UNSECURED_TRANSPORT", "true");

var mysql = builder.AddMySql("mysql")
    .WithPhpMyAdmin()
    .WithLifetime(ContainerLifetime.Persistent);

// ★★★ 这里的变量 redis 类型是 IResourceBuilder<RedisResource> ★★★
var redis = builder.AddRedis("redis")
    .WithImage("redis", "latest")
    .WithEndpoint(port: 6380, targetPort: 6379, name: "tcp")
    .WithBindMount(@"D:\Docker\RedisData", "/data")
    .WithArgs("--appendonly", "yes", "--requirepass", "123456");

var username = builder.AddParameter("username", secret: true);
var password = builder.AddParameter("password", secret: true);
var rabbitmq = builder.AddRabbitMQ("rabbitmq", username, password, 5672)
    .WithManagementPlugin()
    .WithLifetime(ContainerLifetime.Persistent);

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

builder.AddQuestionService(mysql, redis, rabbitmq, daprSidecarOptions);

builder.AddSearchService(es, daprSidecarOptions);

builder.AddHotService(redis, rabbitmq, daprSidecarOptions);

builder.AddFeedService(mysql, rabbitmq, daprSidecarOptions);

// 注册 Agent 服务
builder.AddAgentService(mysql, redis, rabbitmq, daprSidecarOptions);

builder.Build().Run();