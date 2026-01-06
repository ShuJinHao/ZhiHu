using Zhihu.AgentService.Infrastructure;
using Zhihu.AgentService.UseCases;
using Zhihu.HttpApi.Common;

var builder = WebApplication.CreateBuilder(args);
builder.AddServiceDefaults();

builder.Configuration.AddDaprConfiguration(["appid.json", "openai.json"]);

builder.Services
    .AddInfrastructureServices(builder.Configuration)
    .AddUseCaseService()
    .AddHttpApiCommon(builder.Configuration);

var app = builder.Build();

app.UseHttpApiCommon();

app.Run();