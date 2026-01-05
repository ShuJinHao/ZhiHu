using Microsoft.AspNetCore.Mvc;
using Zhihu.HotService.UseCases.Managers;
using Zhihu.HttpApi.Common.Infrastructure;

namespace Zhihu.HotService.HttpApi.Controllers;

public class HotController : ApiControllerBase
{
    /// <summary>
    /// 获取热榜问题列表
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public async Task<IActionResult> GetHotList([FromServices] HotRankManager hotRankManager)
    {
        var result = await hotRankManager.GetHotListAsync();

        return ReturnResult(result);
    }
}