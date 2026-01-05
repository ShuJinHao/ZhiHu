using System.Net;
using Dapr.Client;
using Microsoft.Extensions.Configuration;
using Zhihu.SharedKernel.Result;
using Zhihu.SharedModels.Question;

namespace Zhihu.HotService.Infrastructure;

public class QuestionHttpRepository(DaprClient daprClient, IConfiguration configuration) : IQuestionHttpRepository
{
    public string AppId { get; set; } = configuration["AppId:QuestionService"] ?? throw new ArgumentNullException(nameof(AppId));
    public string BaseRouter { get; set; } = "api/question";

    public async Task<Result<List<QuestionInfoDto>?>> GetQuestionInfoListAsync(int[] ids)
    {
        var methodName = $"{BaseRouter}/hot?ids={string.Join(",", ids.Select(n => n.ToString()))}";
        try
        {
            var result = await daprClient.InvokeMethodAsync<List<QuestionInfoDto>>(HttpMethod.Get, AppId, methodName);
            if (result is null) Result.NotFound();
            return Result.Success(result);
        }
        catch (InvocationException ex)
        {
            if (ex.Response.StatusCode == HttpStatusCode.NotFound) return Result.NotFound();
        }
        return Result.Failure();
    }

    public async Task<Result<List<QuestionStatDto>?>> GetLatestQuestionStatListAsync(DateTimeOffset createdAtBegin, DateTimeOffset lastModifiedBegin)
    {
        var methodName = $"{BaseRouter}/stat?createdAtBegin={createdAtBegin:s}&lastModifiedBegin={lastModifiedBegin:s}";
        try
        {
            var result = await daprClient.InvokeMethodAsync<List<QuestionStatDto>>(HttpMethod.Get, AppId, methodName);
            if (result is null) Result.NotFound();
            return Result.Success(result);
        }
        catch (InvocationException ex)
        {
            if (ex.Response.StatusCode == HttpStatusCode.NotFound) return Result.NotFound();
        }
        return Result.Failure();
    }
}