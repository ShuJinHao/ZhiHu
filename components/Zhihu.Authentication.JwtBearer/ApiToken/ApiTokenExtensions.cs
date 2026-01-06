using Microsoft.AspNetCore.Authentication;
using Zhihu.Authentication.JwtBearer;

namespace Zhihu.Authentication.JwtBearer.ApiToken;

public static class ApiTokenExtensions
{
    public static AuthenticationBuilder AddApiToken(this AuthenticationBuilder builder,  Action<ApiTokenAuthOptions> configureOptions)
    {
        return builder.AddScheme<ApiTokenAuthOptions, ApiTokenAuthHandler>(AuthenticationScheme.ApiToken, configureOptions);
    }
}