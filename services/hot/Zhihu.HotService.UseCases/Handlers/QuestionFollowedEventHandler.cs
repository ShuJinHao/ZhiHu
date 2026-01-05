using MediatR;
using Zhihu.SharedModels.Question;
using Zhihu.HotService.UseCases.Managers;

namespace Zhihu.HotService.UseCases.Handlers;

public class QuestionFollowedEventHandler(QuestionStatManager questionStatManager)
    : INotificationHandler<FollowQuestionAddedEvent>
{
    public Task Handle(FollowQuestionAddedEvent notification, CancellationToken cancellationToken)
    {
        questionStatManager.AddFollowCount(notification.QuestionId);
        return Task.CompletedTask;
    }
}
