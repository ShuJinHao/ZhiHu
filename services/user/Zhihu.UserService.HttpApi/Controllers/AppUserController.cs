using Microsoft.AspNetCore.Mvc;
using Zhihu.HttpApi.Common.Infrastructure;
using Zhihu.UserService.UseCases.Commands;
using Zhihu.UserService.UseCases.Queries;

namespace Zhihu.UserService.HttpApi.Controllers;

[Route("api/appuser")]
public class AppUserController : ApiControllerBase
{
    /// <summary>
    /// 获取指定用户信息
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{id:int}")]
    public async Task<IActionResult> Get(int id)
    {
        var result = await Sender.Send(new GetUserInfoQuery(id));

        return ReturnResult(result);
    }

    /// <summary>
    /// 关注指定用户
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpPost("follow/user/{id:int}")]
    public async Task<IActionResult> CreateFolloweeUser(int id)
    {
        var result = await Sender.Send(new CreateFolloweeUserCommand(id));

        return ReturnResult(result);
    }

    /// <summary>
    /// 取关指定用户
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpDelete("follow/user/{id:int}")]
    public async Task<IActionResult> DeletefolloweeUser(int id)
    {
        var result = await Sender.Send(new DeleteFolloweeUserCommand(id));

        return ReturnResult(result);
    }

    /// <summary>
    /// 获取指定用户的所有粉丝
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{id:int}/followers")]
    public async Task<IActionResult> GetFollowerUsers(int id)
    {
        var result = await Sender.Send(new GetFollowerUsersQuery(id));

        return ReturnResult(result);
    }
}