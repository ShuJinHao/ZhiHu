using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Zhihu.HttpApi.Common.Infrastructure;
using Zhihu.SearchService.UseCases.Queries;
using Zhihu.SharedKernel.Paging;

namespace Zhihu.SearchService.HttpApi.Controllers;

[Microsoft.AspNetCore.Components.Route("api/search")]
public class SearchController : ApiControllerBase
{
    /// <summary>
    /// 搜索问答内容
    /// </summary>
    /// <param name="query"></param>
    /// <param name="pagination"></param>
    /// <returns></returns>
    [HttpGet("content/{query}")]
    public async Task<IActionResult> GetContent(string query, [FromQuery] Pagination pagination)
    {
        var result = await Sender.Send(new GetContentQuery(query, pagination));
        Response.Headers.Append("Pagination", JsonSerializer.Serialize(result.Value?.MetaData));
        return ReturnResult(result);
    }

    /// <summary>
    /// 搜索用户
    /// </summary>
    /// <param name="query"></param>
    /// <param name="pagination"></param>
    /// <returns></returns>
    [HttpGet("user/{query}")]
    public async Task<IActionResult> GetUser(string query, [FromQuery] Pagination pagination)
    {
        var result = await Sender.Send(new GetAppUserQuery(query, pagination));
        Response.Headers.Append("Pagination", JsonSerializer.Serialize(result.Value?.MetaData));
        return ReturnResult(result);
    }
}