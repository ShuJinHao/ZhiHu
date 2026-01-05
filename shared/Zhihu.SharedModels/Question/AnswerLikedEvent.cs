using Zhihu.SharedKernel.Domain;

namespace Zhihu.SharedModels.Question;

public class AnswerLikedEvent(int questionId) : BaseEvent
{
    public int QuestionId { get; set; } = questionId;
}
