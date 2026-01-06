using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace Zhihu.Authentication.JwtBearer;

public static class AuthenticationScheme
{
    public const string AppUserJwtBearer = JwtBearerDefaults.AuthenticationScheme;
    public const string RobotJwtBearer = nameof(RobotJwtBearer);
    public const string ApiToken = nameof(ApiToken);
    public const string Multiple = nameof(Multiple);
}