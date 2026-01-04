using Zhihu.SharedKernel.Domain;

namespace Zhihu.SharedModels.Question;

public class QuestionViewedEvent(int questionId) : BaseEvent
{
    public int QuestionId { get; set; } = questionId;
}
