using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi; // 【关键】：只保留这个根命名空间，v3.1.1 核心类都在这里

namespace Zhihu.HttpApi.Common.Infrastructure;

public sealed class BearerSecuritySchemeTransformer(IAuthenticationSchemeProvider authenticationSchemeProvider) : IOpenApiDocumentTransformer
{
    public async Task TransformAsync(OpenApiDocument document, OpenApiDocumentTransformerContext context, CancellationToken cancellationToken)
    {
        var authenticationSchemes = await authenticationSchemeProvider.GetAllSchemesAsync();

        if (authenticationSchemes.Any(authScheme => authScheme.Name == JwtBearerDefaults.AuthenticationScheme))
        {
            // 1. 定义 Scheme (直接使用根命名空间下的类)
            var jwtScheme = new OpenApiSecurityScheme
            {
                Type = SecuritySchemeType.Http,
                Scheme = JwtBearerDefaults.AuthenticationScheme.ToLower(), // "bearer"
                In = ParameterLocation.Header,
                BearerFormat = "Json Web Token",
                Description = "JWT Authorization header using the Bearer scheme."
            };

            // 2. 注册到 Components
            document.Components ??= new OpenApiComponents();

            // 【修正1】：显式定义 Value 类型为接口 IOpenApiSecurityScheme
            // 你的报错显示 SecuritySchemes 属性类型是 IDictionary<string, IOpenApiSecurityScheme>
            document.Components.SecuritySchemes = new Dictionary<string, IOpenApiSecurityScheme>
            {
                [JwtBearerDefaults.AuthenticationScheme] = jwtScheme
            };

            // 3. 应用到 Operation
            if (document.Paths != null)
            {
                foreach (var path in document.Paths.Values)
                {
                    foreach (var operation in path.Operations)
                    {
                        operation.Value.Security ??= new List<OpenApiSecurityRequirement>();

                        // 【修正2】：构建引用对象
                        // 报错提示 OpenApiSecuritySchemeReference 需要参数 (string, OpenApiDocument?, string?)
                        var reference = new OpenApiSecuritySchemeReference(JwtBearerDefaults.AuthenticationScheme, document);

                        // 【修正3】：必须使用引用对象作为 Key
                        var requirement = new OpenApiSecurityRequirement
                        {
                            [reference] = new List<string>() // 【修正4】：必须是 List<string>，不能是数组
                        };

                        operation.Value.Security.Add(requirement);
                    }
                }
            }
        }
    }
}