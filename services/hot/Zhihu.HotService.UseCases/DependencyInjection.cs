using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Zhihu.HotService.UseCases.Managers;
using Zhihu.Infrastructure.Quartz;
using Zhihu.UseCases.Common;
using Zhihu.HotService.UseCases.Jobs;

namespace Zhihu.HotService.UseCases;

public static class DependencyInjection
{
    public static IServiceCollection AddUseCaseService(this IServiceCollection services)
    {
        services.AddUseCaseCommon(Assembly.GetExecutingAssembly());

        services.AddSingleton<QuestionStatManager>();

        services.AddTransient<HotRankManager>();

        services.AddKeyedTransient<IJobScheduleBuilder, UpdateHotRankJobSchedule>(SchedulerDefinition.ClusteredScheduler);
        services.AddKeyedTransient<IJobScheduleBuilder, RefreshHotRankJobSchedule>(SchedulerDefinition.ClusteredScheduler);

        return services;
    }
}