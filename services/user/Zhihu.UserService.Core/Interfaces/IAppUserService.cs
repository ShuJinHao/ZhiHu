using Zhihu.SharedKernel.Result;
using Zhihu.UserService.Core.Entities;

namespace Zhihu.UserService.Core.Interfaces;

public interface IAppUserService
{
    Task<Result> FolloweeUserAsync(AppUser appuser, int foloweeId, CancellationToken cancellationToken);
}