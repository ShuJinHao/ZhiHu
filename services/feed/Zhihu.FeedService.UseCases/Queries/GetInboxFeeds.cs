using Zhihu.FeedService.Infrastructure.Contexts;
using Zhihu.Infrastructure.EFCore;
using Zhihu.SharedKernel.Paging;
using Zhihu.SharedModels.Feed;

namespace Zhihu.FeedService.UseCases.Queries;

public record GetInboxFeedsQuery(int Userid, Pagination Pagination) : IQuery<Result<PagedList<FeedDto>>>;

public class GetInboxFeedsQueryHandler(FeedReadDbContext dbContext) : IQueryHandler<GetInboxFeedsQuery, Result<PagedList<FeedDto>>>
{
    public async Task<Result<PagedList<FeedDto>>> Handle(GetInboxFeedsQuery request, CancellationToken cancellationToken)
    {
        var queryable = from feed in dbContext.Inbox.AsQueryable()
            where feed.UserId == request.Userid
            orderby feed.ReceivedAt descending
            select new FeedDto
            {
                FeedId = feed.FeedId,
                FeedType = feed.FeedType,
                SenderId = feed.SenderId
            };

        var result = await queryable.ToPageListAsync(request.Pagination);

        return result == null ? Result.NotFound() : Result.Success(result);
    }
}
