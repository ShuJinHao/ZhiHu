using FluentValidation;
using Zhihu.Core.Common.Interfaces;
using Zhihu.SharedKernel.Messaging;
using Zhihu.SharedKernel.Repository;
using Zhihu.SharedKernel.Result;
using Zhihu.UserService.Core.Entities;
using Zhihu.UserService.Core.Interfaces;
using Zhihu.UserService.Core.Specifications;

namespace Zhihu.UserService.UseCases.Commands;

public record CreateFolloweeUserCommand(int FolloweeId) : ICommand<Result>;

public class CreateFolloweeUserCommandValidator : AbstractValidator<CreateFolloweeUserCommand>
{
    public CreateFolloweeUserCommandValidator()
    {
        RuleFor(command => command.FolloweeId)
            .GreaterThan(0);
    }
}

public class CreateFolloweeUserCommandHandler(
    IRepository<AppUser> userRepo,
    IAppUserService appUserService,
    IUser user) : ICommandHandler<CreateFolloweeUserCommand, Result>
{
    public async Task<Result> Handle(CreateFolloweeUserCommand request, CancellationToken cancellationToken)
    {
        var spec = new FolloweeUserByIdSpec(user.Id!.Value, request.FolloweeId);
        var appuser = await userRepo.GetSingleOrDefaultAsync(spec, cancellationToken);
        if (appuser == null) return Result.NotFound("用户不存在");

        var result = await appUserService.FolloweeUserAsync(appuser, request.FolloweeId, cancellationToken);
        if (!result.IsSuccess) return result;

        await userRepo.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}