using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Zhihu.SharedKernel.Messaging;
using Zhihu.SharedKernel.Result;
using Zhihu.UseCases.Common.Attributes;
using Zhihu.UserService.Infrastructure.Contexts;

namespace Zhihu.UserService.UseCases.Queries;

public record GetFollowerUsersQuery(int UserId) : IQuery<Result<List<int>>>;

public class GetFollowerUsersQueryValidator : AbstractValidator<GetFollowerUsersQuery>
{
    public GetFollowerUsersQueryValidator()
    {
        RuleFor(command => command.UserId)
            .GreaterThan(0);
    }
}

public class GetFollowerUsersQueryHandler(UserReadDbContext dbContext)
    : IQueryHandler<GetFollowerUsersQuery, Result<List<int>>>
{
    public async Task<Result<List<int>>> Handle(GetFollowerUsersQuery request, CancellationToken cancellationToken)
    {
        var queryable = dbContext.FollowUsers.AsNoTracking()
            .Where(u => u.FolloweeId == request.UserId)
            .Select(u => u.FollowerId);
        // 2 关注 1，FollowerId=2，FolloweeId=1
        // 3 关注 1，FollowerId=3，FolloweeId=1
        var result = await queryable.ToListAsync(cancellationToken: cancellationToken);

        return Result.Success(result);
    }
}