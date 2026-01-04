using Zhihu.QuestionService.UseCases.Questions;

namespace Zhihu.QuestionService.UseCases.Answers;

public record AnswerWithQuestionDto
{
    public AnswerDto Answer { get; init; } = null!;

    public QuestionDto Question { get; init; } = null!;
}