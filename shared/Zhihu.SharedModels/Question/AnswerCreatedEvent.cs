using Zhihu.SharedKernel.Domain;

namespace Zhihu.SharedModels.Question;

public class AnswerCreatedEvent(int questionId) : BaseEvent
{
    public int QuestionId { get; set; } = questionId;
}