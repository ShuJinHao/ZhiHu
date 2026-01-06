using Microsoft.EntityFrameworkCore;
using Zhihu.AgentService.Infrastructure.Contexts;
using Zhihu.AgentService.MigrationWorker;

var builder = Host.CreateApplicationBuilder(args);

builder.AddServiceDefaults();

builder.Services.AddHostedService<Worker>();

builder.Services.AddOpenTelemetry()
    .WithTracing(tracing => tracing.AddSource(Worker.ActivitySourceName));

var connectionString = builder.Configuration.GetConnectionString("MasterDb");

builder.Services.AddDbContext<AgentDbContext>(options =>
{
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
});

var host = builder.Build();
host.Run();