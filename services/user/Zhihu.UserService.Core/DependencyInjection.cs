using Microsoft.Extensions.DependencyInjection;
using Zhihu.UserService.Core.Interfaces;
using Zhihu.UserService.Core.Services;

namespace Zhihu.UserService.Core;

public static class DependencyInjection
{
    public static IServiceCollection AddCoreServices(this IServiceCollection services)
    {
        services.AddScoped<IAppUserService, AppUserService>();
        return services;
    }
}