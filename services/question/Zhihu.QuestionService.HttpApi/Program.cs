using Zhihu.HttpApi.Common;
using Zhihu.QuestionService.Infrastructure;
using Zhihu.QuestionService.UseCases;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.Configuration.AddDaprConfiguration();

builder.Services
    .AddInfrastructureServices(builder.Configuration)
    .AddUseCaseServices()
    .AddHttpApiCommon(builder.Configuration);

var app = builder.Build();

app.UseHttpApiCommon();

app.Run();