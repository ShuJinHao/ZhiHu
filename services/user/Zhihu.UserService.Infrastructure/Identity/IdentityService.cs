using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Zhihu.SharedKernel.Result;
using Zhihu.SharedModels.Enums;
using Zhihu.SharedModels.Identity;

namespace Zhihu.UserService.Infrastructure.Identity;

public class IdentityService(
    UserManager<IdentityUser> userManager,
    IOptions<JwtSettings> jwtSettings)
{
    public async Task<IResult> CreateUserAsync(string username, string password)
    {
        var user = new IdentityUser
        {
            UserName = username,
            Email = username
        };

        var identityResult = await userManager.CreateAsync(user, password);

        return identityResult.Succeeded
            ? Result.Success(user.Id)
            : Result.Failure(identityResult.Errors.Select(e => e.Description).ToArray());
    }

    public async Task<Result<string>> GetAccessTokenAsync(string username, string password)
    {
        // 验证用户名和密码
        var user = await userManager.FindByNameAsync(username);
        if (user is null || !await userManager.CheckPasswordAsync(user, password)) return Result.Unauthorized();

        // 创建 JWT
        var jwt = new JwtSecurityToken(
            jwtSettings.Value.Issuer,
            jwtSettings.Value.Audience,
            [
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.UserName!),
                new Claim(nameof(UserType), UserType.AppUser.ToString())
            ],
            expires: DateTime.Now.AddMinutes(jwtSettings.Value.AccessTokenExpirationMinutes),
            signingCredentials: new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Value.Secret!)),
                SecurityAlgorithms.HmacSha256)
        );

        // 生成 JWT 字符串
        var token = new JwtSecurityTokenHandler().WriteToken(jwt);

        return token is null ? Result.Failure() : Result.Success(token);
    }
}