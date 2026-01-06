using Microsoft.AspNetCore.Authentication;
using Zhihu.Authentication.JwtBearer;

namespace Zhihu.Authentication.JwtBearer.Multiple;

public static class MultipleAuthExtensions
{
    public static AuthenticationBuilder AddMultipleAuth(this AuthenticationBuilder builder)
    {
        return builder.AddScheme<AuthenticationSchemeOptions, MultipleAuthHandler>(AuthenticationScheme.Multiple, null);
    }
}