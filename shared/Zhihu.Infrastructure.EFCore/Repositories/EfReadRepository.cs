using Microsoft.EntityFrameworkCore;
using Zhihu.SharedKernel.Domain;
using Zhihu.SharedKernel.Repository;
using Zhihu.SharedKernel.Specification;

namespace Zhihu.Infrastructure.EFCore.Repositories;

public abstract class EfReadRepository<T>(DbContext dbContext) : IReadRepository<T> where T : class, IEntity
{
    protected readonly DbSet<T> DbSet = dbContext.Set<T>();

    public async Task<T?> GetByIdAsync<TKey>(TKey id, CancellationToken cancellationToken = default)
    {
        return await DbSet.FindAsync(id, cancellationToken);
    }

    public async Task<List<T>> GetListAsync(ISpecification<T>? specification = null,
        CancellationToken cancellationToken = default)
    {
        return await SpecificationEvaluator.GetQuery(DbSet, specification).ToListAsync(cancellationToken);
    }

    public async Task<T?> GetSingleOrDefaultAsync(ISpecification<T>? specification = null,
        CancellationToken cancellationToken = default)
    {
        return await SpecificationEvaluator.GetQuery(DbSet, specification).FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<int> CountAsync(ISpecification<T>? specification = null,
        CancellationToken cancellationToken = default)
    {
        return await SpecificationEvaluator.GetQuery(DbSet, specification).CountAsync(cancellationToken);
    }

    public async Task<bool> AnyAsync(ISpecification<T>? specification = null,
        CancellationToken cancellationToken = default)
    {
        return await SpecificationEvaluator.GetQuery(DbSet, specification).AnyAsync(cancellationToken);
    }
}