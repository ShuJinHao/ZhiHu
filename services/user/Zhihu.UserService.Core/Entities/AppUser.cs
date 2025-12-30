using Zhihu.Core.Common;
using Zhihu.Core.Common.Entities;
using Zhihu.SharedKernel.Domain;
using Zhihu.SharedKernel.Result;

namespace Zhihu.UserService.Core.Entities;

public class AppUser : BaseAuditEntity, IAggregateRoot
{
    private readonly List<FollowUser> _followees = [];

    private readonly List<FollowUser> _followers = [];

    public AppUser()
    {
    }

    public AppUser(int userId)
    {
        Id = userId;
    }

    public string? Nickname { get; set; }

    public string? Avatar { get; set; }

    public string? Bio { get; set; }

    /// <summary>
    ///     关注列表
    /// </summary>
    public IReadOnlyCollection<FollowUser> Followees => _followees.AsReadOnly();

    /// <summary>
    ///     粉丝列表
    /// </summary>
    public IReadOnlyCollection<FollowUser> Followers => _followers.AsReadOnly();

    /// <summary>
    ///     添加关注
    /// </summary>
    /// <param name="followeeId"></param>
    /// <returns></returns>
    public Result AddFollowee(int followeeId)
    {
        if (_followees.Any(fu => fu.FolloweeId == followeeId)) return Result.Invalid("该用户已关注");

        var followUser = new FollowUser
        {
            FollowerId = Id,
            FolloweeId = followeeId,
            FollowDate = DateTimeOffset.Now
        };

        _followees.Add(followUser);

        return Result.Success();
    }

    /// <summary>
    ///     移除关注
    /// </summary>
    /// <param name="followeeId"></param>
    public void RemoveFollowee(int followeeId)
    {
        var followUser = _followees.FirstOrDefault(fu => fu.FolloweeId == followeeId);
        if (followUser == null) return;

        _followees.Remove(followUser);
    }
}