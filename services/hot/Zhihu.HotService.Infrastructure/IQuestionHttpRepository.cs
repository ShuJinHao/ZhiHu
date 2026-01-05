using Zhihu.SharedKernel.Repository;
using Zhihu.SharedKernel.Result;
using Zhihu.SharedModels.Question;

namespace Zhihu.HotService.Infrastructure;

public interface IQuestionHttpRepository : IHttpRepository
{
    Task<Result<List<QuestionInfoDto>?>> GetQuestionInfoListAsync(int[] ids);
    
    Task<Result<List<QuestionStatDto>?>> GetLatestQuestionStatListAsync(DateTimeOffset createdAtBegin, DateTimeOffset lastModifiedBegin);
}