using CommunityToolkit.Aspire.Hosting.Dapr;
using Zhihu.AppHost;

var builder = DistributedApplication.CreateBuilder(args);

var mysql = builder.AddMySql("mysql")
    .WithPhpMyAdmin()
    .WithLifetime(ContainerLifetime.Persistent);

var redis = builder.AddRedis("redis")
    .WithRedisInsight()
    .WithLifetime(ContainerLifetime.Persistent);

var username = builder.AddParameter("username", secret: true);
var password = builder.AddParameter("password", secret: true);
var rabbitmq = builder.AddRabbitMQ("rabbitmq", username, password, 5672)
    .WithManagementPlugin()
    .WithLifetime(ContainerLifetime.Persistent);

var daprSidecarOptions = new DaprSidecarOptions
{
    ResourcesPaths = ["./DaprComponents"]
};

builder.AddUserService(mysql, daprSidecarOptions);

builder.AddQuestionService(mysql, redis, rabbitmq, daprSidecarOptions);

builder.Build().Run();