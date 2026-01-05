using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Quartz;
using Quartz.Impl;
using Quartz.Simpl;
using Quartz.Spi;

namespace Zhihu.Infrastructure.Quartz;

public static class DependencyInjection
{
    public static void AddQuartzService(this IServiceCollection services
        , QuartzOption quartzOption)
    {
        foreach (var schedulerOption in quartzOption.Schedulers)
        {
            var scheduler = SchedulerBuilder
                .Create(schedulerOption.ToNameValueCollection())
                .BuildScheduler().Result;

            services.AddSingleton(scheduler);
        }

        services.TryAddSingleton<ISchedulerFactory, StdSchedulerFactory>();
        services.TryAddSingleton<IJobFactory, MicrosoftDependencyInjectionJobFactory>();

        if (!services.Any(x => x.ImplementationType == typeof(QuartzHostedService)))
        {
            services.AddHostedService<QuartzHostedService>();
        }
    }
}