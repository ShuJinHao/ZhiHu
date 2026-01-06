using MediatR;
using Zhihu.FeedService.UseCases.Commands;
using Zhihu.SharedModels.Feed;

namespace Zhihu.FeedService.UseCases.Handlers;

public class FeedCreatedEventHandler(ISender sender) : INotificationHandler<FeedCreatedEvent>
{
    public async Task Handle(FeedCreatedEvent notification, CancellationToken cancellationToken)
    {
        // 写发件箱（可选）
        await sender.Send(new CreateOutboxFeedCommand(
            notification.UserId,
            notification.FeedId,
            notification.FeedType), cancellationToken);

        // 写收件箱
        await sender.Send(new CreateInboxFeedsCommand(
            notification.UserId,
            notification.FeedId,
            notification.FeedType), cancellationToken);
    }
}