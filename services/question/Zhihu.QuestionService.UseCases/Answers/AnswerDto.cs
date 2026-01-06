using Zhihu.SharedModels.Enums;

namespace Zhihu.QuestionService.UseCases.Answers;

public record AnswerDto
{
    public int Id { get; init; }

    public string? Content { get; init; }

    public int LikeCount { get; init; }

    public DateTimeOffset? LastModifiedAt { get; init; }

    public int? CreatedBy { get; init; }

    public UserType CreatedByType { get; init; }
}