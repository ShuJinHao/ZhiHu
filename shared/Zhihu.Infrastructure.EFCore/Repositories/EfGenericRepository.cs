using Microsoft.EntityFrameworkCore;
using Zhihu.SharedKernel.Domain;
using Zhihu.SharedKernel.Repository;

namespace Zhihu.Infrastructure.EFCore.Repositories;

public class EfGenericRepository<T>(DbContext dbContext)
    : EfReadRepository<T>(dbContext), IGenericRepository<T> where T : class, IEntity
{
    private readonly DbContext _dbContext = dbContext;

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await _dbContext.SaveChangesAsync(cancellationToken);
    }
}