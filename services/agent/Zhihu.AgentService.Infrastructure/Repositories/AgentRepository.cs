using Zhihu.Infrastructure.EFCore.Repositories;
using Zhihu.SharedKernel.Domain;
using Zhihu.AgentService.Infrastructure.Contexts;

namespace Zhihu.AgentService.Infrastructure.Repositories;

public class AgentRepository<T>(AgentDbContext dbContext) : EfRepository<T>(dbContext) where T : class, IEntity, IAggregateRoot
{
    
}