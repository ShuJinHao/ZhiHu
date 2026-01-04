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

        services.AddOpenApi(options =>
        {
            options.AddDocumentTransformer((document, context, cancellationToken) =>
            {
                // 1. 初始化 Info
                document.Info ??= new Microsoft.OpenApi.OpenApiInfo();
                document.Info.Title = "知乎 API";
                document.Info.Version = "v1";

                // 2. 初始化 Components
                document.Components ??= new Microsoft.OpenApi.OpenApiComponents();

                // ★★★ 核心修复：显式使用 IOpenApiSecurityScheme 接口初始化字典 ★★★
                // 既然 Interfaces 命名空间不存在，那么 IOpenApiSecurityScheme 一定在根目录下
                document.Components.SecuritySchemes ??= new Dictionary<string, Microsoft.OpenApi.IOpenApiSecurityScheme>();

                // 3. 定义 Scheme (具体类 OpenApiSecurityScheme 实现了 IOpenApiSecurityScheme)
                var securityScheme = new Microsoft.OpenApi.OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = Microsoft.OpenApi.SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    In = Microsoft.OpenApi.ParameterLocation.Header,
                    Description = "请输入 Bearer Token"
                };

                if (!document.Components.SecuritySchemes.ContainsKey("Bearer"))
                {
                    document.Components.SecuritySchemes.Add("Bearer", securityScheme);
                }

                // 4. 初始化 Security 列表
                document.Security ??= new List<Microsoft.OpenApi.OpenApiSecurityRequirement>();

                // 5. 添加全局安全要求
                var requirement = new Microsoft.OpenApi.OpenApiSecurityRequirement
        {
            {
                // v2.0 引用写法
                new Microsoft.OpenApi.OpenApiSecuritySchemeReference("Bearer", document),
                new List<string>() // 必须是 List
            }
        };

                document.Security.Add(requirement);

                return Task.CompletedTask;
            });
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