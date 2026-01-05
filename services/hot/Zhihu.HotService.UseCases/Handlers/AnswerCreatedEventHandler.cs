using MediatR;
using Zhihu.SharedModels.Question;
using Zhihu.HotService.UseCases.Managers;

namespace Zhihu.HotService.UseCases.Handlers;

public class AnswerCreatedEventHandler(QuestionStatManager questionStatManager)
    : INotificationHandler<AnswerCreatedEvent>
{
    public Task Handle(AnswerCreatedEvent notification, CancellationToken cancellationToken)
    {
        questionStatManager.AddAnswerCount(notification.QuestionId);
        return Task.CompletedTask;
    }
}