using Microsoft.AspNetCore.Authentication;

namespace Zhihu.Authentication.JwtBearer.ApiToken;

public class ApiTokenAuthOptions : AuthenticationSchemeOptions
{
    public string HeaderName { get; set; } = "dapr-api-token"; // 默认请求头名称
    public string ApiToken { get; set; } = string.Empty;
}