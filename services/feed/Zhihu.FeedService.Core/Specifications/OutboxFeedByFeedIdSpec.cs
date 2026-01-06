using Zhihu.SharedKernel.Specification;
using Zhihu.SharedModels.Feed;
using Zhihu.FeedService.Core.Entities;

namespace Zhihu.FeedService.Core.Specifications;

public class OutboxFeedByFeedIdSpec : Specification<Outbox>
{
    public OutboxFeedByFeedIdSpec(int feedId, FeedType feedType)
    {
        FilterCondition = f => f.FeedId == feedId && f.FeedType == feedType;
    }
}
