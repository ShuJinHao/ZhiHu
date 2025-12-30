using Zhihu.SharedKernel.Domain;

namespace Zhihu.SharedModels.Question;

public class AnswerLikedEvent : BaseEvent
{
    public int QuestionId { get; set; }
}