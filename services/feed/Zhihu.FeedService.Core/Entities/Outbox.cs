using Zhihu.SharedKernel.Domain;
using Zhihu.SharedModels.Feed;

namespace Zhihu.FeedService.Core.Entities;

public class Outbox : BaseEntity<int>, IAggregateRoot
{
    public int UserId { get; set; }

    public int FeedId { get; set; }

    public FeedType FeedType { get; set; }

    public DateTimeOffset SentAt { get; set; } = DateTimeOffset.Now;
}