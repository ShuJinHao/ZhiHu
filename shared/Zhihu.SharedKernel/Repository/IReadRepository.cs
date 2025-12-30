using Zhihu.SharedKernel.Domain;
using Zhihu.SharedKernel.Specification;

namespace Zhihu.SharedKernel.Repository;

/// <summary>
///     <para>
///         <see cref="IRepository{T}" /> 用于查询 <typeparamref name="T" />.
///     </para>
/// </summary>
/// <typeparam name="T">该仓储操作的实体类型</typeparam>
public interface IReadRepository<T> where T : class, IEntity
{
    /// <summary>
    ///     查询具有指定主键的实体
    /// </summary>
    /// <typeparam name="TKey">主键的类型</typeparam>
    /// <param name="id">要查找的实体的主键值</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<T?> GetByIdAsync<TKey>(TKey id, CancellationToken cancellationToken = default);

    /// <summary>
    ///     查询实体集合
    /// </summary>
    /// <param name="specification"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<List<T>> GetListAsync(ISpecification<T>? specification = null, CancellationToken cancellationToken = default);

    /// <summary>
    ///     查询实体
    /// </summary>
    /// <param name="specification"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<T?> GetSingleOrDefaultAsync(ISpecification<T>? specification = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     统计符合条件的记录总数
    /// </summary>
    /// <param name="specification"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<int> CountAsync(ISpecification<T>? specification = null, CancellationToken cancellationToken = default);

    /// <summary>
    ///     是否存在符合条件的记录
    /// </summary>
    /// <param name="specification"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<bool> AnyAsync(ISpecification<T>? specification = null, CancellationToken cancellationToken = default);
}