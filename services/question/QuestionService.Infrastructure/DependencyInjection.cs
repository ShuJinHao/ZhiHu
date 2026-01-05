using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Zhihu.Infrastructure.Cache;
using Zhihu.Infrastructure.EFCore;
using Zhihu.Infrastructure.MessageBus;
using Zhihu.Infrastructure.Quartz;
using Zhihu.QuestionService.Core.Data;
using Zhihu.QuestionService.Infrastructure.Contexts;
using Zhihu.QuestionService.Infrastructure.Repositories;
using Zhihu.SharedKernel.Repository;

namespace Zhihu.QuestionService.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services,
        IConfiguration configuration)
    {
        ConfigureEfCore(services, configuration);

        ConfigureCache(services, configuration);

        ConfigureQuartz(services);

        services.AddMessageBusService();

        return services;
    }

    private static void ConfigureEfCore(IServiceCollection services, IConfiguration configuration)
    {
        services.AddInfrastructureEfCore<QuestionDbContext, QuestionReadDbContext>(configuration);
        services.AddScoped(typeof(IReadRepository<>), typeof(QuestionReadRepository<>));
        services.AddScoped(typeof(IRepository<>), typeof(QuestionRepository<>));
        services.AddScoped<IAnswerRepository, AnswerRepository>();
    }

    private static void ConfigureCache(IServiceCollection services, IConfiguration configuration)
    {
        var redisConn = configuration.GetConnectionString("redis");
        services.AddCache(redisConn);
    }

    private static void ConfigureQuartz(this IServiceCollection services)
    {
        var quartzOption = new QuartzOption
        {
            Schedulers =
            [
                new SchedulerOption
                {
                    ["quartz.scheduler.instanceName"] = SchedulerDefinition.LocalScheduler,
                    ["quartz.jobStore.type"] = "Quartz.Simpl.RAMJobStore, Quartz"
                }
            ]
        };

        services.AddQuartzService(quartzOption);
    }
}