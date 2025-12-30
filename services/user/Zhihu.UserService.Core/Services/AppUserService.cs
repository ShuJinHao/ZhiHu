using Zhihu.SharedKernel.Repository;
using Zhihu.SharedKernel.Result;
using Zhihu.UserService.Core.Specifications;
using Zhihu.UserService.Core.Entities;
using Zhihu.UserService.Core.Interfaces;

namespace Zhihu.UserService.Core.Services;

public class AppUserService(IReadRepository<AppUser> appUsers) : IAppUserService
{
    /// <summary>
    ///     关注用户
    /// </summary>
    /// <param name="appuser">用户</param>
    /// <param name="followeeId">关注用户ID</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<Result> FolloweeUserAsync(AppUser appuser, int followeeId, CancellationToken cancellationToken)
    {
        if (await appUsers.CountAsync(new AppUserByIdSpec(followeeId), cancellationToken) == 0)
            return Result.NotFound("关注用户不存在");

        return appuser.AddFollowee(followeeId);
    }
}