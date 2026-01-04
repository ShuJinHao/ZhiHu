namespace Zhihu.QuestionService.UseCases.Answers;

public record AnswerDto
{
    public int Id { get; init; }

    public string? Content { get; init; }

    public int LikeCount { get; init; }

    public DateTimeOffset? LastModifiedAt { get; init; }

    public int? CreatedBy { get; init; }

    public string? CreatedByNickName { get; init; }

    public string? CreatedByBio { get; init; }
}