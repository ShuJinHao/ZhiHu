using Zhihu.Infrastructure.EFCore.Repositories;
using Zhihu.SharedKernel.Domain;
using Zhihu.FeedService.Infrastructure.Contexts;

namespace Zhihu.FeedService.Infrastructure.Repositories;

public class FeedRepository<T>(FeedDbContext dbContext) : EfRepository<T>(dbContext) where T : class, IEntity, IAggregateRoot
{
    
}
