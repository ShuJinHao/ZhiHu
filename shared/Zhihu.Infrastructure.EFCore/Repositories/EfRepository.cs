using Microsoft.EntityFrameworkCore;
using Zhihu.SharedKernel.Domain;
using Zhihu.SharedKernel.Repository;
using Zhihu.SharedKernel.Specification;

namespace Zhihu.Infrastructure.EFCore.Repositories;

public abstract class EfRepository<T>(DbContext dbContext) : EfReadRepository<T>(dbContext), IRepository<T>
    where T : class, IEntity, IAggregateRoot
{
    private readonly DbContext _dbContext = dbContext;

    public T Add(T entity)
    {
        DbSet.Add(entity);
        return entity;
    }

    public void Update(T entity)
    {
        _dbContext.Entry(entity).State = EntityState.Modified;
        DbSet.Update(entity);
    }

    public void Delete(T entity)
    {
        DbSet.Remove(entity);
    }

    public async Task<int> BatchDeleteAsync(ISpecification<T>? specification = null,
        CancellationToken cancellationToken = default)
    {
        return await SpecificationEvaluator.GetQuery(DbSet, specification).ExecuteDeleteAsync(cancellationToken);
    }

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await _dbContext.SaveChangesAsync(cancellationToken);
    }
}