using Zhihu.FeedService.Core.Entities;
using Zhihu.FeedService.Infrastructure.Repositories;
using Zhihu.SharedKernel.Repository;
using Zhihu.SharedModels.Feed;

namespace Zhihu.FeedService.UseCases.Commands;

public record CreateInboxFeedsCommand(int SenderId, int FeedId, FeedType FeedType) : ICommand<Result>;

public class CreateInboxFeedsHandler(
    IMapper mapper,
    IUserHttpRepository userRepo,
    IRepository<Inbox> inbox) : ICommandHandler<CreateInboxFeedsCommand, Result>
{
    public async Task<Result> Handle(CreateInboxFeedsCommand request, CancellationToken cancellationToken)
    {
        var result = await userRepo.GetFollowerIdsAsync(request.SenderId);
        
        if (!result.IsSuccess) return Result.Failure("获取粉丝失败");
        
        var followerIds = result.Value!;
        
        if (followerIds.Count == 0) return Result.Success();

        foreach (var id in followerIds)
        {
            var feed = mapper.Map<Inbox>(request);
            feed.UserId = id;
            inbox.Add(feed);
        }

        await inbox.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
}
