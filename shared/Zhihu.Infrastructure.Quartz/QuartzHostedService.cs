using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Quartz;
using Quartz.Spi;

namespace Zhihu.Infrastructure.Quartz;

public class QuartzHostedService(ISchedulerFactory schedulerFactory, 
    IJobFactory jobFactory, IServiceProvider serviceProvider) : IHostedService
{
    private IReadOnlyList<IScheduler>? AllSchedulers { get; } = schedulerFactory.GetAllSchedulers().Result;

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        if (AllSchedulers == null) return;

        foreach (var scheduler in AllSchedulers)
        {
            scheduler.JobFactory = jobFactory;
            
            var scheduleBuilders = serviceProvider.GetKeyedServices<IJobScheduleBuilder>(scheduler.SchedulerName);
            foreach (var scheduleBuilder in scheduleBuilders)
            {
                scheduleBuilder.CreateJobSchedule(scheduler);
            }
            await scheduler.StartDelayed(TimeSpan.FromSeconds(5), cancellationToken);
        }
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        if (AllSchedulers == null) return;
        foreach (var scheduler in AllSchedulers)
        {
            await scheduler.Shutdown(cancellationToken);
        }
    }
}
