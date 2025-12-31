using FluentValidation;
using Zhihu.Core.Common.Interfaces;
using Zhihu.SharedKernel.Messaging;
using Zhihu.SharedKernel.Repository;
using Zhihu.SharedKernel.Result;
using Zhihu.UserService.Core.Entities;
using Zhihu.UserService.Core.Specifications;

namespace Zhihu.UserService.UseCases.Commands;

public record DeleteFolloweeUserCommand(int FolloweeId) : ICommand<Result>;

public class DeleteFolloweeUserCommandValidator : AbstractValidator<DeleteFolloweeUserCommand>
{
    public DeleteFolloweeUserCommandValidator()
    {
        RuleFor(command => command.FolloweeId)
            .GreaterThan(0);
    }
}

public class DeleteFolloweeUserCommandHandler(
    IRepository<AppUser> appusers,
    IUser user) : ICommandHandler<DeleteFolloweeUserCommand, Result>
{
    public async Task<Result> Handle(DeleteFolloweeUserCommand request, CancellationToken cancellationToken)
    {
        var spec = new FolloweeUserByIdSpec(user.Id!.Value, request.FolloweeId);
        var appuser = await appusers.GetSingleOrDefaultAsync(spec, cancellationToken);
        if (appuser == null) return Result.NotFound("用户不存在");

        appuser.RemoveFollowee(request.FolloweeId);

        await appusers.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}