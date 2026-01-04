using Zhihu.Infrastructure.EFCore.Repositories;
using Zhihu.QuestionService.Infrastructure.Contexts;
using Zhihu.SharedKernel.Domain;

namespace Zhihu.QuestionService.Infrastructure.Repositories;

public class QuestionRepository<T>(QuestionDbContext dbContext) : EfRepository<T>(dbContext) where T : class, IEntity, IAggregateRoot
{
    
}