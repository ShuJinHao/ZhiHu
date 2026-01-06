using System.Security.Claims;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Zhihu.Authentication.JwtBearer.ApiToken;

public class ApiTokenAuthHandler(
    IOptionsMonitor<ApiTokenAuthOptions> options,
    ILoggerFactory logger,
    UrlEncoder encoder)
    : AuthenticationHandler<ApiTokenAuthOptions>(options, logger, encoder)
{
    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {

        if (!Request.Headers.TryGetValue(Options.HeaderName, out var extractedApiKey))
        {
            return Task.FromResult(AuthenticateResult.NoResult());
        }
        
        if (Options.ApiToken != extractedApiKey)
        {
            return Task.FromResult(AuthenticateResult.NoResult());
        }
        
        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, "0"),
            new Claim(ClaimTypes.Role, "Dapr"),
            new Claim(ClaimTypes.Name, "Dapr")
        };
        var identity = new ClaimsIdentity(claims, Scheme.Name);
        var principal = new ClaimsPrincipal(identity);
        var ticket = new AuthenticationTicket(principal, Scheme.Name);

        return Task.FromResult(AuthenticateResult.Success(ticket));
    }
}