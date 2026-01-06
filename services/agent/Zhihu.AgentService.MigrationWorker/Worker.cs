using OpenTelemetry.Trace;
using System.Diagnostics;
using Zhihu.AgentService.Infrastructure.Contexts;
using Zhihu.Infrastructure.EFCore;

namespace Zhihu.AgentService.MigrationWorker;

public class Worker(IHostApplicationLifetime applicationLifetime, IServiceProvider serviceProvider) : BackgroundService
{
    public const string ActivitySourceName = "UserService Migrations";
    private static readonly ActivitySource ActivitySource = new(ActivitySourceName);

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using var activity = ActivitySource.StartActivity("Migrating database", ActivityKind.Client);

        try
        {
            using var scope = serviceProvider.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<AgentDbContext>();
            await MigrationTool.RunMigrationAsync(dbContext, stoppingToken);
        }
        catch (Exception ex)
        {
            activity?.RecordException(ex);
            throw;
        }

        applicationLifetime.StopApplication();
    }
}