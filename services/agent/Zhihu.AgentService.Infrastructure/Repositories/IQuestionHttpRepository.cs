using Zhihu.SharedKernel.Repository;
using Zhihu.SharedKernel.Result;

namespace Zhihu.AgentService.Infrastructure.Repositories;

public interface IQuestionHttpRepository : IHttpRepository
{
    Task<IResult> CreateAnswerAsync(int questionId, string content, string token);
}