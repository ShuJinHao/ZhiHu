using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;
using ZiggyCreatures.Caching.Fusion;

namespace Zhihu.Infrastructure.Cache;

public static class DependencyInjection
{
    public static IServiceCollection AddCache(this IServiceCollection services,
        string? redisConn)
    {
        ArgumentNullException.ThrowIfNull(redisConn);   
        services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect(redisConn));
        services.AddStackExchangeRedisCache(options => options.Configuration = redisConn);
        services.AddFusionCache()
            .WithOptions(options =>
            {
                options.DefaultEntryOptions = new FusionCacheEntryOptions
                {
                    Duration = TimeSpan.FromMinutes(1)
                };
            })
            .WithSystemTextJsonSerializer()
            .WithDistributedCache(provider => provider.GetRequiredService<IDistributedCache>());

        services.AddSingleton(typeof(ICacheService<>), typeof(CacheService<>));
        return services;
    }
}
