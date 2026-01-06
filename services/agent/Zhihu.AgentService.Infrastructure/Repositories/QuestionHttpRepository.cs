using System.Net;
using System.Net.Http.Headers;
using Dapr.Client;
using Microsoft.Extensions.Configuration;
using Zhihu.SharedKernel.Result;

namespace Zhihu.AgentService.Infrastructure.Repositories;

public class QuestionHttpRepository(DaprClient daprClient, IConfiguration configuration) : IQuestionHttpRepository
{
    public string AppId { get; set; } = configuration["AppId:QuestionService"] ?? throw new ArgumentNullException(nameof(AppId));
    public string BaseRouter { get; set; } = "api/question";
    
    public async Task<IResult> CreateAnswerAsync(int questionId, string answer, string token)
    {
        var methodName = $"{BaseRouter}/{questionId}/answer";
        
        try
        {
            var request = daprClient.CreateInvokeMethodRequest(HttpMethod.Post,
                AppId,
                methodName,
                null,
                new { Content = answer });
            
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
           
            await daprClient.InvokeMethodAsync(request);
            
            return Result.Success();
        }
        catch (InvocationException ex)
        {
            if (ex.Response.StatusCode != HttpStatusCode.OK) return Result.Failure();
        }
        return Result.Failure();
    }
}