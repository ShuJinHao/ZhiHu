using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Zhihu.Infrastructure.EFCore.Interceptors;

namespace Zhihu.Infrastructure.EFCore;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureEfCore<TMasterDbContext, TSlaveDbContext>(
            this IServiceCollection services,
            IConfiguration configuration)
            where TMasterDbContext : DbContext
            where TSlaveDbContext : DbContext
    {
        var masterDbConn = configuration.GetConnectionString("MasterDb");
        var slaveDbConn = configuration.GetConnectionString("SlaveDb");

        services.AddScoped<AuditEntityInterceptor>();
        services.AddScoped<DispatchDomainEventsInterceptor>();

        services.AddDbContext<TMasterDbContext>((sp, options) =>
        {
            options.AddInterceptors(sp.GetRequiredService<AuditEntityInterceptor>());
            options.AddInterceptors(sp.GetRequiredService<DispatchDomainEventsInterceptor>());
            options.UseMySql(masterDbConn, ServerVersion.AutoDetect(masterDbConn));
        });

        services.AddDbContext<TSlaveDbContext>((sp, options) =>
        {
            options.AddInterceptors(sp.GetRequiredService<DispatchDomainEventsInterceptor>());
            options.UseMySql(slaveDbConn, ServerVersion.AutoDetect(slaveDbConn));
        });

        return services;
    }
}