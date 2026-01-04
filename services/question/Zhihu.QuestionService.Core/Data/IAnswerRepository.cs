using Zhihu.SharedKernel.Repository;
using Zhihu.QuestionService.Core.Entities;
using Zhihu.QuestionService.Core.Specifications;

namespace Zhihu.QuestionService.Core.Data;

public interface IAnswerRepository : IGenericRepository<Answer>
{
    Task<Answer?> GetAnswerByIdWithLikeByUserIdAsync(AnswerByIdWithLikeByUserIdSpec specification,
        CancellationToken cancellationToken = default);
}