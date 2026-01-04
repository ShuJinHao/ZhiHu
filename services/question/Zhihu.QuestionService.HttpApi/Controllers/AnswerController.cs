using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Zhihu.HttpApi.Common.Infrastructure;
using Zhihu.QuestionService.UseCases.Answers.Commands;
using Zhihu.QuestionService.UseCases.Answers.Queries;
using Zhihu.SharedKernel.Paging;

namespace Zhihu.QuestionService.HttpApi.Controllers;

public record CreateAnswerRequest(string Content);

public record UpdateAnswerRequest(string Content);

[Route("api/question/{questionId:int}/answer")]
public class AnswerController : ApiControllerBase
{
    /// <summary>
    /// 回答指定问题
    /// </summary>
    /// <param name="questionId">问题ID</param>
    /// <param name="request">回答内容</param>
    /// <returns></returns>
    [HttpPost]
    public async Task<IActionResult> Create(int questionId, CreateAnswerRequest request)
    {
        var result = await Sender.Send(new CreateAnswerCommand(questionId, request.Content));

        return ReturnResult(result);
    }

    /// <summary>
    /// 删除指定问题的指定回答
    /// </summary>
    /// <param name="questionId">问题ID</param>
    /// <param name="answerId">回答ID</param>
    /// <returns></returns>
    [HttpDelete("{answerId:int}")]
    public async Task<IActionResult> Delete(int questionId, int answerId)
    {
        var result = await Sender.Send(new DeleteAnswerCommand(questionId, answerId));

        return ReturnResult(result);
    }

    /// <summary>
    /// 更新指定问题的指定回答
    /// </summary>
    /// <param name="questionId">问题ID</param>
    /// <param name="answerId">回答ID</param>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPut("{answerId:int}")]
    public async Task<IActionResult> Update(int questionId, int answerId, UpdateAnswerRequest request)
    {
        var result = await Sender.Send(new UpdateAnswerCommand(questionId, answerId, request.Content));

        return ReturnResult(result);
    }

    /// <summary>
    /// 获取指定回答内容
    /// </summary>
    /// <param name="questionId"></param>
    /// <param name="answerId"></param>
    /// <returns></returns>
    [HttpGet("{answerId:int}")]
    public async Task<IActionResult> Get(int questionId, int answerId)
    {
        var result = await Sender.Send(new GetAnswerWithQuestionQuery(questionId, answerId));

        return ReturnResult(result);
    }

    /// <summary>
    /// 获取指定问题的回答列表
    /// </summary>
    /// <param name="questionId"></param>
    /// <param name="pagination"></param>
    /// <returns></returns>
    [HttpGet("list")]
    public async Task<IActionResult> GetList(int questionId, [FromQuery] Pagination pagination)
    {
        var result =
            await Sender.Send(new GetAnswersQuery(questionId, pagination));
        Response.Headers.Append("Pagination", JsonSerializer.Serialize(result.Value?.MetaData));
        return ReturnResult(result);
    }

    /// <summary>
    /// 为指定回答点赞
    /// </summary>
    /// <param name="questionId"></param>
    /// <param name="answerId"></param>
    /// <param name="isLike"></param>
    /// <returns></returns>
    [HttpPost("{answerId:int}/like")]
    public async Task<IActionResult> CreateLike(int questionId, int answerId, bool isLike)
    {
        var result = await Sender.Send(new CreateAnswerLikeCommand(questionId, answerId, isLike));

        return ReturnResult(result);
    }

    /// <summary>
    /// 更新指定回答的点赞
    /// </summary>
    /// <param name="questionId"></param>
    /// <param name="answerId"></param>
    /// <param name="isLike"></param>
    /// <returns></returns>
    [HttpPut("{answerId:int}/like")]
    public async Task<IActionResult> UpdateLike(int questionId, int answerId, bool isLike)
    {
        var result = await Sender.Send(new UpdateAnswerLikeCommand(questionId, answerId, isLike));

        return ReturnResult(result);
    }

    /// <summary>
    /// 删除指定回答的点赞
    /// </summary>
    /// <param name="questionId"></param>
    /// <param name="answerId"></param>
    /// <returns></returns>
    [HttpDelete("{answerId:int}/like")]
    public async Task<IActionResult> DeleteLike(int questionId, int answerId)
    {
        var result = await Sender.Send(new DeleteAnswerLikeCommand(questionId, answerId));

        return ReturnResult(result);
    }
}