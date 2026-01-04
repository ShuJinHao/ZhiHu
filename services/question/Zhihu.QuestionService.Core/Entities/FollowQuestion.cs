using Zhihu.Core.Common;
using Zhihu.Core.Common.Entities;

namespace Zhihu.QuestionService.Core.Entities;

public class FollowUser : BaseEntity
{
    public int QuestionId { get; set; }

    public Question Question { get; set; } = null!;

    public int UserId { get; set; }

    public DateTimeOffset FollowDate { get; set; }
}