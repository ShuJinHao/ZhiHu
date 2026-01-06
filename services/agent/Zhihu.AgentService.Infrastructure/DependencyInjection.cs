using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.SemanticKernel;
using Zhihu.Authentication.JwtBearer;
using Zhihu.Infrastructure.Cache;
using Zhihu.Infrastructure.EFCore;
using Zhihu.SharedKernel.Repository;
using Zhihu.AgentService.Infrastructure.Contexts;
using Zhihu.AgentService.Infrastructure.Repositories;
using Zhihu.AgentService.Infrastructure.Tools;

namespace Zhihu.AgentService.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services,
        IConfiguration configuration)
    {
        ConfigureEfCore(services, configuration);

        ConfigureSemanticKernel(services, configuration);

        ConfigureCache(services, configuration);
        
        ConfigureJwtSettings(services, configuration);

        services.AddSingleton<IQuestionHttpRepository, QuestionHttpRepository>();

        services.AddSingleton<RobotJwtGenerator>();

        return services;
    }

    private static void ConfigureEfCore(IServiceCollection services, IConfiguration configuration)
    {
        services.AddInfrastructureEfCore<AgentDbContext, AgentReadDbContext>(configuration);
        
        services.AddScoped(typeof(IRepository<>), typeof(AgentRepository<>));
    }

    private static void ConfigureSemanticKernel(IServiceCollection services, IConfiguration configuration)
    {
        var client = new HttpClient();
        client.Timeout = TimeSpan.FromMinutes(2);
        
        var builder = Kernel.CreateBuilder();
        builder.AddOpenAIChatCompletion(
            modelId: configuration["OpenAI:ModelId"] ?? throw new ArgumentNullException("ModelId"),
            endpoint: new Uri(configuration["OpenAI:Endpoint"] ?? throw new ArgumentNullException("Endpoint")),
            apiKey: configuration["OpenAI:ApiKey"] ?? throw new ArgumentNullException("ApiKey"),
            httpClient: client);
        
        services.AddTransient(_ => builder.Build());
    }
    
    private static void ConfigureCache(IServiceCollection services, IConfiguration configuration)
    {
        var redisConn = configuration.GetConnectionString("redis");
        services.AddCache(redisConn);
    }
    
    private static void ConfigureJwtSettings(IServiceCollection services, IConfiguration configuration)
    {
        var configurationSection = configuration.GetSection("JwtSettings");
        var jwtSettings = configurationSection.Get<JwtSettings>();
        if (jwtSettings is null) throw new NullReferenceException(nameof(jwtSettings));
        services.Configure<JwtSettings>(configurationSection);
    }
}