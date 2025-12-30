using Zhihu.Infrastructure.EFCore.Repositories;
using Zhihu.SharedKernel.Domain;
using Zhihu.UserService.Infrastructure.Contexts;

namespace Zhihu.UserService.Infrastructure.Repositories;

public class UserReadRepository<T>(UserReadDbContext dbContext) : EfReadRepository<T>(dbContext) where T : class, IEntity
{
}