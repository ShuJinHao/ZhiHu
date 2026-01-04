using System.Reflection;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Zhihu.Authentication.JwtBearer;
using Zhihu.Core.Common.Interfaces;
using Zhihu.HttpApi.Common.Infrastructure;
using Zhihu.HttpApi.Common.Services;

namespace Zhihu.HttpApi.Common;

public static class DependencyInjection
{
    public static IServiceCollection AddHttpApiCommon(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDaprClient(builder => builder.UseJsonSerializationOptions(new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower
        }));
        services.AddJwtBearer(configuration);

        // 2. 关键：加上这句，让原生 OpenAPI 能扫描到 Controller
        services.AddEndpointsApiExplorer();

        // 3. 原生 OpenAPI 注册 (不要加任何 Transformer 参数！)
        services.AddOpenApi();

        services.AddControllers()
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower;
            });

        services.AddScoped<IUser, CurrentUser>();

        services.AddHttpContextAccessor();

        services.AddExceptionHandler<UseCaseExceptionHandler>();

        services.AddProblemDetails();

        ConfigureCors(services);

        return services;
    }

    private static void ConfigureCors(IServiceCollection services)
    {
        services.AddCors(options =>
        {
            options.AddPolicy("AllowAny", builder =>
            {
                builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader();
            });
        });
    }
}