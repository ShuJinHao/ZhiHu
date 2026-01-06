using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Zhihu.Authentication.JwtBearer;
using Zhihu.SharedModels.Enums;

namespace Zhihu.AgentService.Infrastructure.Tools;

public class RobotJwtGenerator(IOptions<JwtSettings> jwtSettings)
{
    public string GetAccessToken(int id, string name)
    {
        var jwt = new JwtSecurityToken(
            jwtSettings.Value.Issuer,
            "robot",
            [
                new Claim(ClaimTypes.NameIdentifier, id.ToString()),
                new Claim(ClaimTypes.Name, name),
                new Claim(nameof(UserType), UserType.Robot.ToString())
            ],
            signingCredentials: new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Value.Secret)),
                SecurityAlgorithms.HmacSha256)
        );
        
        var token = new JwtSecurityTokenHandler().WriteToken(jwt);
        if (token is null) throw new Exception("生成 JWT 失败");
        return token;
    }
}