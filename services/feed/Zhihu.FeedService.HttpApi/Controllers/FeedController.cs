using MediatR;
using Microsoft.AspNetCore.Mvc;
using Zhihu.FeedService.UseCases.Queries;
using Zhihu.SharedKernel.Paging;

namespace Zhihu.FeedService.HttpApi.Controllers;

[Route("api/feed")]
public class FeedController(ISender sender) : ControllerBase
{
    [HttpGet("{userid:int}")]
    public async Task<IActionResult> GetList(int userid, [FromQuery] Pagination pagination)
    {
        var result = await sender.Send(new GetInboxFeedsQuery(userid, pagination));

        return result.IsSuccess ? Ok(result.Value) : NotFound();
    }
}