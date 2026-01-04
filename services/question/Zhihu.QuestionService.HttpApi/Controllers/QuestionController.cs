using Microsoft.AspNetCore.Mvc;
using Zhihu.HttpApi.Common.Infrastructure;
using Zhihu.QuestionService.UseCases.Questions.Commands;
using Zhihu.QuestionService.UseCases.Questions.Queries;

namespace Zhihu.QuestionService.HttpApi.Controllers;

public record CreateQuestionRequest(string Title, string Description);

public record UpdateQuestionRequest(string Title, string Description);

[Route("api/question")]
public class QuestionController : ApiControllerBase
{
    /// <summary>
    /// 创建问题
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<IActionResult> Create(CreateQuestionRequest request)
    {
        var result = await Sender.Send(new CreateQuestionCommand(request.Title, request.Description));

        return ReturnResult(result);
    }

    /// <summary>
    /// 删除指定问题
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await Sender.Send(new DeleteQuestionCommand(id));

        return ReturnResult(result);
    }

    /// <summary>
    /// 更新指定问题
    /// </summary>
    /// <param name="id"></param>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, UpdateQuestionRequest request)
    {
        var result = await Sender.Send(new UpdateQuestionCommand(id, request.Title, request.Description));

        return ReturnResult(result);
    }

    /// <summary>
    /// 获取指定问题内容
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{id:int}")]
    public async Task<IActionResult> Get(int id)
    {
        var result = await Sender.Send(new GetQuestionQuery(id));

        return ReturnResult(result);
    }
    
}
