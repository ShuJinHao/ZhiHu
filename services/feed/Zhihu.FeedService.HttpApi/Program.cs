using Zhihu.FeedService.Infrastructure;
using Zhihu.FeedService.UseCases;
using Zhihu.HttpApi.Common;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.Configuration.AddDaprConfiguration(["appid.json"]);

builder.Services
    .AddInfrastructureServices(builder.Configuration)
    .AddUseCaseServices()
    .AddHttpApiCommon(builder.Configuration);

var app = builder.Build();

app.UseHttpApiCommon();

app.Run();