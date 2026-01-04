using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Zhihu.UseCases.Common;

namespace Zhihu.QuestionService.UseCases;

public static class DependencyInjection
{
    public static IServiceCollection AddUseCaseServices(this IServiceCollection services)
    {
        services.AddUseCaseCommon(Assembly.GetExecutingAssembly());
        return services;
    }
}