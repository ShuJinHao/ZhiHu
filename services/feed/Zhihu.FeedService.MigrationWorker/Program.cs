using Microsoft.EntityFrameworkCore;
using Zhihu.FeedService.Infrastructure.Contexts;
using Zhihu.FeedService.MigrationWorker;

var builder = Host.CreateApplicationBuilder(args);

builder.AddServiceDefaults();

builder.Services.AddHostedService<Worker>();

builder.Services.AddOpenTelemetry()
    .WithTracing(tracing => tracing.AddSource(Worker.ActivitySourceName));

var connectionString = builder.Configuration.GetConnectionString("MasterDb");

builder.Services.AddDbContext<FeedDbContext>(options =>
{
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
});

var host = builder.Build();
host.Run();