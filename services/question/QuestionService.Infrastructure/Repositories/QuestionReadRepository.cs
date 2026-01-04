using Zhihu.Infrastructure.EFCore.Repositories;
using Zhihu.QuestionService.Infrastructure.Contexts;
using Zhihu.SharedKernel.Domain;

namespace Zhihu.QuestionService.Infrastructure.Repositories;

public class QuestionReadRepository<T>(QuestionReadDbContext dbContext) : EfReadRepository<T>(dbContext) where T : class, IEntity
{
    
}