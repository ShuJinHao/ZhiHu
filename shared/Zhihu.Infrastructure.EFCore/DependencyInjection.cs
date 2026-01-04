using Microsoft.Extensions.DependencyInjection;
using Zhihu.Infrastructure.EFCore.Interceptors;

namespace Zhihu.Infrastructure.EFCore;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureEfCore(this IServiceCollection services)
    {
        services.AddScoped<AuditEntityInterceptor>();
        services.AddScoped<DispatchDomainEventsInterceptor>();

        return services;
    }
}