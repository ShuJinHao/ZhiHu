using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi.Interfaces; // 👈 关键：v3 引入了大量接口
using Microsoft.OpenApi.Models;

namespace Zhihu.HttpApi.Common.Infrastructure;

public sealed class BearerSecuritySchemeTransformer(IAuthenticationSchemeProvider authenticationSchemeProvider) : IOpenApiDocumentTransformer
{
    public async Task TransformAsync(OpenApiDocument document, OpenApiDocumentTransformerContext context, CancellationToken cancellationToken)
    {
        var authenticationSchemes = await authenticationSchemeProvider.GetAllSchemesAsync();

        // 检查是否有 JWT Bearer 认证方案
        if (authenticationSchemes.Any(authScheme => authScheme.Name == JwtBearerDefaults.AuthenticationScheme))
        {
            // 1. 定义 Scheme 对象
            // 注意：在 v3 中，这个对象既是定义，也是后续引用的“句柄”
            var bearerScheme = new OpenApiSecurityScheme
            {
                Type = SecuritySchemeType.Http,
                Scheme = JwtBearerDefaults.AuthenticationScheme.ToLower(), // "bearer"
                In = ParameterLocation.Header,
                BearerFormat = "Json Web Token"
                // ❌ 不要在这里写 Reference = ... 属性已经没了
            };

            // 2. 添加到全局 Components
            document.Components ??= new OpenApiComponents();

            // ✅ 关键修复：显式使用 IOpenApiSecurityScheme 接口
            // v3.1.1 强制要求字典的值是接口类型，不能是具体类
            document.Components.SecuritySchemes = new Dictionary<string, IOpenApiSecurityScheme>
            {
                [JwtBearerDefaults.AuthenticationScheme] = bearerScheme
            };

            // 3. 应用到所有 API 操作
            foreach (var operation in document.Paths.Values.SelectMany(path => path.Operations))
            {
                // ✅ 关键修复：直接使用上面的 bearerScheme 对象作为 Key
                // v3 库会自动识别这个对象已存在于 Components 中，从而在生成 JSON 时自动生成 $ref
                var requirement = new OpenApiSecurityRequirement
                {
                    [bearerScheme] = Array.Empty<string>()
                };

                operation.Value.Security.Add(requirement);
            }
        }
    }
}