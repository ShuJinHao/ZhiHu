using MediatR;
using Zhihu.SharedModels.Question;
using Zhihu.HotService.UseCases.Managers;

namespace Zhihu.HotService.UseCases.Handlers;

public class QuestionViewedEventHandler(QuestionStatManager questionStatManager)
    : INotificationHandler<QuestionViewedEvent>
{
    public Task Handle(QuestionViewedEvent notification, CancellationToken cancellationToken)
    {
        questionStatManager.AddViewCount(notification.QuestionId);
        return Task.CompletedTask;
    }
}
