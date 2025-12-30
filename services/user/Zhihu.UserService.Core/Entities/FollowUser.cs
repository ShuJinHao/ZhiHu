using Zhihu.Core.Common;
using Zhihu.Core.Common.Entities;

namespace Zhihu.UserService.Core.Entities;

public class FollowUser : BaseEntity
{
    // 关注者
    public int FollowerId { get; set; }

    public AppUser Follower { get; set; } = null!;

    // 被关注者
    public int FolloweeId { get; set; }

    public AppUser Followee { get; set; } = null!;

    public DateTimeOffset FollowDate { get; set; }
}