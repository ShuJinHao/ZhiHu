using Zhihu.Infrastructure.EFCore.Repositories;
using Zhihu.SharedKernel.Domain;
using Zhihu.FeedService.Infrastructure.Contexts;

namespace Zhihu.FeedService.Infrastructure.Repositories;

public class FeedReadRepository<T>(FeedReadDbContext dbContext) : EfReadRepository<T>(dbContext) where T : class, IEntity
{
    
}