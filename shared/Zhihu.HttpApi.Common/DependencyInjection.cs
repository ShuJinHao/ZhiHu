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
        services.AddJwtBearer(configuration);

        // 添加OpenAPI服务
        services.AddOpenApi(options =>
        {
            // 注册安全架构转换器
            options.AddDocumentTransformer<BearerSecuritySchemeTransformer>();
        });

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