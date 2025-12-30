using Zhihu.SharedKernel.Specification;
using Zhihu.UserService.Core.Entities;

namespace Zhihu.UserService.Core.Specifications;

public class AppUserByIdSpec : Specification<AppUser>
{
    public AppUserByIdSpec(int userId)
    {
        FilterCondition = user => user.Id == userId;
    }
}