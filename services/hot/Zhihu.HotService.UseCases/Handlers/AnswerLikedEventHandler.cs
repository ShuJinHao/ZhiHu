using MediatR;
using Zhihu.SharedModels.Question;
using Zhihu.HotService.UseCases.Managers;

namespace Zhihu.HotService.UseCases.Handlers;

public class AnswerLikedEventHandler(QuestionStatManager questionStatManager)
    : INotificationHandler<AnswerLikedEvent>
{
    public Task Handle(AnswerLikedEvent notification, CancellationToken cancellationToken)
    {
        questionStatManager.AddLikeCount(notification.QuestionId);
        return Task.CompletedTask;
    }
}
