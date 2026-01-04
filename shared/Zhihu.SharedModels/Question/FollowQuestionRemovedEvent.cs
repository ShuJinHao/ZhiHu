using Zhihu.SharedKernel.Domain;

namespace Zhihu.SharedModels.Question;

internal class FollowQuestionRemovedEvent : BaseEvent
{
    public int QuestionId { get; set; }
}