using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Zhihu.Infrastructure.EFCore;
using Zhihu.Infrastructure.EFCore.Interceptors;
using Zhihu.SharedKernel.Repository;
using Zhihu.SharedModels.Identity;
using Zhihu.UserService.Infrastructure.Contexts;
using Zhihu.UserService.Infrastructure.Identity;
using Zhihu.UserService.Infrastructure.Repositories;

namespace Zhihu.UserService.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services,
        IConfiguration configuration)
    {
        ConfigureEfCore(services, configuration);

        ConfigureIdentity(services, configuration);

        return services;
    }

    private static void ConfigureEfCore(IServiceCollection services, IConfiguration configuration)
    {
        services.AddInfrastructureEfCore();

        var masterDb = configuration.GetConnectionString("MasterDb");
        var slaveDb = configuration.GetConnectionString("SlaveDb");

        services.AddDbContext<UserDbContext>((sp, options) =>
        {
            options.AddInterceptors(sp.GetRequiredService<AuditEntityInterceptor>());
            options.AddInterceptors(sp.GetRequiredService<DispatchDomainEventsInterceptor>());
            options.UseMySql(masterDb, ServerVersion.AutoDetect(masterDb));
        });

        services.AddDbContext<UserReadDbContext>((sp, options) =>
        {
            options.AddInterceptors(sp.GetRequiredService<DispatchDomainEventsInterceptor>());
            options.UseMySql(slaveDb, ServerVersion.AutoDetect(slaveDb));
        });

        services.AddScoped(typeof(IReadRepository<>), typeof(UserReadRepository<>));
        services.AddScoped(typeof(IRepository<>), typeof(UserRepository<>));
    }

    private static void ConfigureIdentity(IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddIdentityCore<IdentityUser>(options =>
            {
                options.User.RequireUniqueEmail = true;
                options.Password.RequiredLength = 8;
            })
            .AddEntityFrameworkStores<UserDbContext>();

        services.AddScoped<IdentityService>();

        // 从配置文件中读取JwtSettings，并注入到容器中
        var configurationSection = configuration.GetSection("JwtSettings");
        var jwtSettings = configurationSection.Get<JwtSettings>();
        if (jwtSettings is null) throw new NullReferenceException(nameof(jwtSettings));
        services.Configure<JwtSettings>(configurationSection);
    }
}