using Zhihu.Core.Common;
using Zhihu.Core.Common.Entities;

namespace Zhihu.QuestionService.Core.Entities;

public class AnswerLike : BaseAuditEntity
{
    public int AnswerId { get; set; }
    public Answer Answer { get; set; } = null!;

    public int UserId { get; set; }

    public bool IsLike { get; set; }
}