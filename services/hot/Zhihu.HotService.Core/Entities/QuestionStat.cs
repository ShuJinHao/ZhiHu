namespace Zhihu.HotService.Core.Entities;

public record QuestionStat
{
    public int ViewCount { get; set; }

    public int AnswerCount { get; set; }

    public int LikeCount { get; set; }

    public int FollowCount { get; set; }
}
