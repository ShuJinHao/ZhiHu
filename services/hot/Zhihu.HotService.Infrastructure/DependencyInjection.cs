using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;
using Zhihu.Infrastructure.Quartz;

namespace Zhihu.HotService.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services,
        IConfiguration configuration)
    {
        ConfigureRedis(services, configuration);

        ConfigureQuartz(services, configuration);

        services.AddSingleton<IQuestionHttpRepository, QuestionHttpRepository>();

        return services;
    }

    private static void ConfigureRedis(IServiceCollection services, IConfiguration configuration)
    {
        var redisConn = configuration.GetConnectionString("redis");
        ArgumentNullException.ThrowIfNull(redisConn);
        services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect(redisConn));
    }

    private static void ConfigureQuartz(IServiceCollection services, IConfiguration configuration)
    {
        var quartzOption = configuration.GetSection("Quartz").Get<QuartzOption>();
        ArgumentNullException.ThrowIfNull(quartzOption);
        services.AddQuartzService(quartzOption);
    }
}