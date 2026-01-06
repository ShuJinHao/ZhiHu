using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Zhihu.FeedService.Infrastructure.Contexts;
using Zhihu.FeedService.Infrastructure.Repositories;
using Zhihu.Infrastructure.EFCore;
using Zhihu.SharedKernel.Repository;

namespace Zhihu.FeedService.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        ConfigureEfCore(services, configuration);
        services.AddSingleton<IUserHttpRepository, UserHttpRepository>();

        return services;
    }

    private static void ConfigureEfCore(IServiceCollection services, IConfiguration configuration)
    {
        services.AddInfrastructureEfCore<FeedDbContext, FeedReadDbContext>(configuration);

        services.AddScoped(typeof(IReadRepository<>), typeof(FeedReadRepository<>));
        services.AddScoped(typeof(IRepository<>), typeof(FeedRepository<>));
    }
}