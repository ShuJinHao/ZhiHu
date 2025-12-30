using Zhihu.SharedKernel.Domain;

namespace Zhihu.SharedModels.Question;

public class FollowQuestionRemovedEvent : BaseEvent
{
    public int QuestionId { get; set; }
}