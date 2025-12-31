using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Zhihu.SharedKernel.Messaging;
using Zhihu.SharedKernel.Result;
using Zhihu.UseCases.Common.Attributes;
using Zhihu.UserService.Infrastructure.Contexts;
using Zhihu.UserService.UseCases;

namespace Zhihu.UserService.UseCases.Queries;

[Authorize]
public record GetUserInfoQuery(int UserId) : IQuery<Result<UserInfoDto>>;

public class GetUserInfoQueryValidator : AbstractValidator<GetUserInfoQuery>
{
    public GetUserInfoQueryValidator()
    {
        RuleFor(command => command.UserId)
            .GreaterThan(0);
    }
}

public class GetUserInfoQueryHandler(UserReadDbContext dbContext)
    : IQueryHandler<GetUserInfoQuery, Result<UserInfoDto>>
{
    public async Task<Result<UserInfoDto>> Handle(GetUserInfoQuery request, CancellationToken cancellationToken)
    {
        var queryable = dbContext.AppUsers
            .Where(u => u.Id == request.UserId)
            .Select(u => new UserInfoDto
            {
                Id = u.Id,
                Nickname = u.Nickname,
                Avatar = u.Avatar,
                Bio = u.Bio,
                FolloweesCount = u.Followees.Count,
                FollowersCount = u.Followers.Count
            });

        var appUserInfo = await queryable.FirstOrDefaultAsync(cancellationToken: cancellationToken);

        return appUserInfo is null ? Result.NotFound() : Result.Success(appUserInfo);
    }
}