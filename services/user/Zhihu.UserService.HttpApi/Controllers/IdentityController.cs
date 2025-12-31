using Microsoft.AspNetCore.Mvc;
using Zhihu.HttpApi.Common.Infrastructure;
using Zhihu.UserService.Infrastructure.Identity;
using Zhihu.UserService.UseCases.Commands;

namespace Zhihu.UserService.HttpApi.Controllers;

public record UserRegisterRequest(string Username, string Password);

public record UserLoginRequest(string Username, string Password);

[Route("/identity")]
public class IdentityController(IdentityService identityService) : ApiControllerBase
{
    /// <summary>
    /// 用户注册
    /// </summary>
    /// <param name="register"></param>
    /// <returns></returns>
    /// <remarks>
    /// 示例请求:
    ///
    ///     POST /identity/login
    ///     {
    ///        "username": "zilor@com",
    ///        "password": "123456aA!"
    ///     }
    ///
    /// </remarks>
    [HttpPost("register")]
    public async Task<IActionResult> Register(UserRegisterRequest register)
    {
        var identityResult = await identityService.CreateUserAsync(register.Username, register.Password);

        if (!identityResult.IsSuccess) return ReturnResult(identityResult);

        var result = await Sender.Send(new CreateAppUserCommand(Convert.ToInt32(identityResult.GetValue())));

        return ReturnResult(result);
    }

    /// <summary>
    /// 用户登录
    /// </summary>
    /// <param name="login">登录请求</param>
    /// <returns></returns>
    /// <remarks>
    /// 示例请求:
    ///
    ///     POST /identity/login
    ///     {
    ///        "username": "zilor@com",
    ///        "password": "123456aA!"
    ///     }
    ///
    /// </remarks>
    [HttpPost("login")]
    public async Task<IActionResult> Login(UserLoginRequest login)
    {
        var result = await identityService.GetAccessTokenAsync(login.Username, login.Password);

        if (!result.IsSuccess) return ReturnResult(result);

        return Ok(new
        {
            AccessToken = result.Value
        });
    }
}
