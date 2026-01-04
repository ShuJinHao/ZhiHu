using Microsoft.EntityFrameworkCore;
using Zhihu.Infrastructure.EFCore.Repositories;
using Zhihu.QuestionService.Core.Data;
using Zhihu.QuestionService.Core.Entities;
using Zhihu.QuestionService.Core.Specifications;
using Zhihu.QuestionService.Infrastructure.Contexts;

namespace Zhihu.QuestionService.Infrastructure.Repositories;

public class AnswerRepository(QuestionDbContext dbContext) : EfGenericRepository<Answer>(dbContext), IAnswerRepository
{
    public async Task<Answer?> GetAnswerByIdWithLikeByUserIdAsync(AnswerByIdWithLikeByUserIdSpec specification,
        CancellationToken cancellationToken = default)
    {
        return await SpecificationEvaluator.GetQuery(DbSet, specification)
            .FirstOrDefaultAsync(cancellationToken);
    }
}
