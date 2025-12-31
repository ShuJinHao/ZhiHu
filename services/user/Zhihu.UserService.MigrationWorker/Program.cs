using Microsoft.EntityFrameworkCore;
using Zhihu.UserService.Infrastructure.Contexts;
using Zhihu.UserService.MigrationWorker;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddOpenTelemetry()
    .WithTracing(tracing => tracing.AddSource(Worker.ActivitySourceName));

var connectionString = builder.Configuration.GetConnectionString("MasterDb");

builder.Services.AddDbContext<UserDbContext>(options =>
{
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
});

builder.Services.AddHostedService<Worker>();

var host = builder.Build();
host.Run();