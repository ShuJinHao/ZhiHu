using Zhihu.Infrastructure.EFCore.Repositories;
using Zhihu.SharedKernel.Domain;
using Zhihu.UserService.Infrastructure.Contexts;

namespace Zhihu.UserService.Infrastructure.Repositories;

public class UserRepository<T>(UserDbContext dbContext) : EfRepository<T>(dbContext) where T : class, IEntity, IAggregateRoot
{
}