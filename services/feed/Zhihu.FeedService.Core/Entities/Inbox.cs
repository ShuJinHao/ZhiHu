using Zhihu.SharedKernel.Domain;
using Zhihu.SharedModels.Feed;

namespace Zhihu.FeedService.Core.Entities;

public class Inbox : BaseEntity<long>, IAggregateRoot
{
    public int UserId { get; set; }

    public int FeedId { get; set; }

    public FeedType FeedType { get; set; }

    public int SenderId { get; set; }

    public DateTimeOffset ReceivedAt { get; set; } = DateTimeOffset.Now;
}