using System.Reflection;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Zhihu.UseCases.Common.Behaviors;

namespace Zhihu.UseCases.Common;

public static class DependencyInjection
{
    public static IServiceCollection AddUseCaseCommon(this IServiceCollection services, Assembly assembly)
    {
        services.AddAutoMapper(assembly);

        services.AddValidatorsFromAssembly(assembly);
        
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(assembly);
            cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(AuthorizationBehavior<,>));
            cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        });
        
        return services;
    }
}