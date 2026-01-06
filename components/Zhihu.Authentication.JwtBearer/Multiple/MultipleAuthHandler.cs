using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Zhihu.Authentication.JwtBearer;

namespace Zhihu.Authentication.JwtBearer.Multiple;

public class MultipleAuthHandler(
    IOptionsMonitor<AuthenticationSchemeOptions> options,
    ILoggerFactory logger,
    UrlEncoder encoder)
    : AuthenticationHandler<AuthenticationSchemeOptions>(options, logger, encoder)
{
    protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        // 尝试用户 JWT 认证
        var userJwtAuth = await Context.AuthenticateAsync(AuthenticationScheme.AppUserJwtBearer);
        if (userJwtAuth.Succeeded)
        {
            return AuthenticateResult.Success(userJwtAuth.Ticket!);
        }
        
        // 如果用户 JWT 认证失败，尝试机器人 JWT 认证
        var robotJwtAuth = await Context.AuthenticateAsync(AuthenticationScheme.RobotJwtBearer);
        if (robotJwtAuth.Succeeded)
        {
            return AuthenticateResult.Success(robotJwtAuth.Ticket!);
        }

        // 如果 JWT 认证失败，尝试 API Key 认证
        var apiKeyAuth = await Context.AuthenticateAsync(AuthenticationScheme.ApiToken);
        if (apiKeyAuth.Succeeded)
        {
            return AuthenticateResult.Success(apiKeyAuth.Ticket!);
        }

        return AuthenticateResult.NoResult();
    }
}