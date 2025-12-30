using Zhihu.SharedKernel.Domain;

namespace Zhihu.SharedModels.Question;

public class QuestionViewedEvent : BaseEvent
{
    public int QuestionId { get; set; }
}