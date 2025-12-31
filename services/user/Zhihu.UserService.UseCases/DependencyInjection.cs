using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Zhihu.UseCases.Common;

namespace Zhihu.UserService.UseCases;

public static class DependencyInjection
{
    public static IServiceCollection AddUseCaseService(this IServiceCollection services)
    {
        services.AddUseCaseCommon(Assembly.GetExecutingAssembly());
        return services;
    }
}