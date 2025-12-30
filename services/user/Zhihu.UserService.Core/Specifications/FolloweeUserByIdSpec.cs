using Zhihu.SharedKernel.Specification;
using Zhihu.UserService.Core.Entities;

namespace Zhihu.UserService.Core.Specifications;

public class FolloweeUserByIdSpec : Specification<AppUser>
{
    public FolloweeUserByIdSpec(int userId, int followeeId)
    {
        FilterCondition = user => user.Id == userId;
        AddInclude(user => user.Followees.Where(followUser => followUser.FolloweeId == followeeId));
    }
}