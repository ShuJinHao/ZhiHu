using Zhihu.SharedKernel.Domain;

namespace Zhihu.SharedModels.Question;

public class FollowQuestionAddedEvent : BaseEvent
{
    public int QuestionId { get; set; }
}
