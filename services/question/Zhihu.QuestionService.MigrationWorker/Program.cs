using Microsoft.EntityFrameworkCore;
using Zhihu.QuestionService.Infrastructure.Contexts;
using Zhihu.QuestionService.MigrationWorker;

var builder = Host.CreateApplicationBuilder(args);
builder.AddServiceDefaults();

builder.Services.AddHostedService<Worker>();

builder.Services.AddOpenTelemetry()
    .WithTracing(tracing => tracing.AddSource(Worker.ActivitySourceName));

var connectionString = builder.Configuration.GetConnectionString("MasterDb");
builder.Services.AddDbContext<QuestionDbContext>(options =>
{
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
});

var host = builder.Build();
host.Run();