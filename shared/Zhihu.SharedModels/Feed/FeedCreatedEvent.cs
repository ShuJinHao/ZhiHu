using Zhihu.SharedKernel.Domain;

namespace Zhihu.SharedModels.Feed;

public enum FeedType { Quesiton, Answer }

public class FeedCreatedEvent : BaseEvent
{
    public FeedType FeedType { get; init; }

    public int FeedId { get; init; }

    public int UserId { get; init; }
};