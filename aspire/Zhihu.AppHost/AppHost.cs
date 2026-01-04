using CommunityToolkit.Aspire.Hosting.Dapr;
using Zhihu.AppHost;

var builder = DistributedApplication.CreateBuilder(args);

// 【新增】强制允许不安全传输，解决 OTLP/Dashboard 报 HTTP 错误的问题
builder.Configuration["ASPIRE_ALLOW_UNSECURED_TRANSPORT"] = "true";

var mysql = builder.AddMySql("mysql")
    .WithPhpMyAdmin()
    .WithLifetime(ContainerLifetime.Persistent);

var daprSidecarOptions = new DaprSidecarOptions
{
    ResourcesPaths = ["./DaprComponents"]
};

builder.AddUserService(mysql, daprSidecarOptions);

builder.Build().Run();