using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using Zhihu.Infrastructure.Quartz;
using Zhihu.QuestionService.UseCases.Questions.Jobs;
using Zhihu.UseCases.Common;

namespace Zhihu.QuestionService.UseCases;

public static class DependencyInjection
{
    public static IServiceCollection AddUseCaseServices(this IServiceCollection services)
    {
        services.AddUseCaseCommon(Assembly.GetExecutingAssembly());
        services.AddKeyedTransient<IJobScheduleBuilder, UpdateQuestionViewCountsJobSchedule>(SchedulerDefinition.LocalScheduler);
        return services;
    }
}