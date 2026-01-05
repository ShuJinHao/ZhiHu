using Zhihu.HotService.Infrastructure;
using Zhihu.HotService.UseCases;
using Zhihu.HttpApi.Common;

var builder = WebApplication.CreateBuilder(args);
builder.AddServiceDefaults();

builder.Configuration.AddDaprConfiguration(["quartz.json", "appid.json"]);

builder.Services
    .AddInfrastructureServices(builder.Configuration)
    .AddUseCaseService()
    .AddHttpApiCommon(builder.Configuration);

var app = builder.Build();

app.UseHttpApiCommon();

app.Run();