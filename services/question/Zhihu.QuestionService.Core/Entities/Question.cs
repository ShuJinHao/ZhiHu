using Zhihu.Core.Common;
using Zhihu.SharedKernel.Domain;
using Zhihu.SharedKernel.Result;
using Zhihu.QuestionService.Core.Data;
using Zhihu.Core.Common.Entities;

namespace Zhihu.QuestionService.Core.Entities;

public class Question : AuditWithUserEntity, IAggregateRoot
{
    public string Title { get; set; } = null!;

    public string? Description { get; set; }

    public string? Summary { get; set; }

    public int ViewCount { get; private set; }

    public int FollowerCount { get; set; }

    public ICollection<Answer> Answers { get; set; } = new List<Answer>();

    private readonly List<FollowUser> _followUsers = [];

    /// <summary>
    ///     问题关注列表
    /// </summary>
    public IReadOnlyCollection<FollowUser> FollowUsers => _followUsers.AsReadOnly();

    /// <summary>
    ///     添加关注问题
    /// </summary>
    /// <param name="questionId"></param>
    /// <returns></returns>
    public Result AddFollowQuestion(int questionId)
    {
        if (_followUsers.Any(fq => fq.QuestionId == questionId)) return Result.Invalid("问题已关注");

        var followQuestion = new FollowUser
        {
            UserId = Id,
            QuestionId = questionId,
            FollowDate = DateTimeOffset.Now
        };

        _followUsers.Add(followQuestion);
        FollowerCount++;

        return Result.Success();
    }

    /// <summary>
    ///     移除关注问题
    /// </summary>
    /// <param name="questionId"></param>
    public void RemoveFollowQuestion(int questionId)
    {
        var followQuestion = _followUsers.FirstOrDefault(fq => fq.QuestionId == questionId);
        if (followQuestion == null) return;

        _followUsers.Remove(followQuestion);
        FollowerCount--;
    }

    public void AddViewCount(int count)
    {
        ViewCount += count;
    }

    public void GenerateSummary()
    {
        if (Description == null) return;

        if (Description.Length <= DataSchemaConstants.DefaultQuestionSummaryLength)
        {
            Summary = Description;
            return;
        }

        Summary = Description?.Substring(0, DataSchemaConstants.DefaultQuestionSummaryLength);
    }
}