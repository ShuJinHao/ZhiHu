using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Zhihu.Authentication.JwtBearer;
using Zhihu.Authentication.JwtBearer.ApiToken;
using Zhihu.Authentication.JwtBearer.Multiple;

namespace Zhihu.Authentication.JwtBearer;

public static class DependencyInjection
{
    public static void AddJwtBearer(this IServiceCollection services, IConfiguration configuration)
    {
        // 从配置文件中读取JwtSettings，并注入到容器中
        var configurationSection = configuration.GetSection("JwtSettings");
        var jwtSettings = configurationSection.Get<JwtSettings>();
        if (jwtSettings is null) throw new NullReferenceException(nameof(jwtSettings));
        services.Configure<JwtSettings>(configurationSection);

        var apiToken = configuration["APP_API_TOKEN"];
        ArgumentNullException.ThrowIfNull(apiToken);

        services.AddAuthentication(AuthenticationScheme.Multiple)
            .AddMultipleAuth()//添加混合验证
            .AddApiToken(options =>
            {
                options.ApiToken = apiToken;
            })//添加api验证
            .AddJwtBearer(AuthenticationScheme.AppUserJwtBearer, options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ClockSkew = TimeSpan.Zero,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtSettings.Issuer,
                    ValidAudience = jwtSettings.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(jwtSettings.Secret)
                    )
                };
            })//添加用户验证
            .AddJwtBearer(AuthenticationScheme.RobotJwtBearer, options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = false,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtSettings.Issuer,
                    ValidAudience = "robot",
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(jwtSettings.Secret)
                    )
                };
            }); //添加机器人验证
    }
}