using Microsoft.Extensions.DependencyInjection;

namespace Zhihu.Infrastructure.MessageBus;

public static class DependencyInjection
{
    public static IServiceCollection AddMessageBusService(this IServiceCollection services)
    {
        services.AddScoped<IMessageBusService, MessageBusService>();
        return services;
    }
}
