using Zhihu.SharedKernel.Domain;

namespace Zhihu.SharedModels.Question;

public class AnswerCreatedEvent : BaseEvent
{
    public int QuestionId { get; set; }
}