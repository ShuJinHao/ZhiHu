using Zhihu.AppHost;

var builder = DistributedApplication.CreateBuilder(args);

var mysql = builder.AddMySql("mysql")
    .WithPhpMyAdmin()
    .WithLifetime(ContainerLifetime.Persistent);

builder.AddUserService(mysql);

builder.Build().Run();