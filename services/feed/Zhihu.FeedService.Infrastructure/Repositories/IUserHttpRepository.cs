using Zhihu.SharedKernel.Repository;
using Zhihu.SharedKernel.Result;

namespace Zhihu.FeedService.Infrastructure.Repositories;

public interface IUserHttpRepository : IHttpRepository
{
    Task<Result<List<int>>> GetFollowerIdsAsync(int userId);
}