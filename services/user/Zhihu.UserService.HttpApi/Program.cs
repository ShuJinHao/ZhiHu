using Zhihu.HttpApi.Common;
using Zhihu.UserService.Core;
using Zhihu.UserService.Infrastructure;
using Zhihu.UserService.UseCases;

// Add services to the container.

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.Services
    .AddCoreServices()
    .AddInfrastructureServices(builder.Configuration)
    .AddUseCaseService()
    .AddHttpApiCommon(builder.Configuration);

var app = builder.Build();

app.UseHttpApiCommon();

app.Run();