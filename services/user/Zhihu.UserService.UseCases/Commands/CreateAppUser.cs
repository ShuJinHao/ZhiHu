using AutoMapper;
using FluentValidation;
using Zhihu.SharedKernel.Messaging;
using Zhihu.SharedKernel.Repository;
using Zhihu.SharedKernel.Result;
using Zhihu.UserService.Core.Entities;
using Zhihu.UserService.UseCases;

namespace Zhihu.UserService.UseCases.Commands;

public record CreateAppUserCommand(int UserId) : ICommand<Result<CreatedAppUserDto>>;

public class CreateAppUserCommandValidator : AbstractValidator<CreateAppUserCommand>
{
    public CreateAppUserCommandValidator()
    {
        RuleFor(command => command.UserId)
            .GreaterThan(0);
    }
}

public class CreateAppUserCommandHandler(
    IRepository<AppUser> userRepo,
    IMapper mapper) : ICommandHandler<CreateAppUserCommand, Result<CreatedAppUserDto>>
{
    public async Task<Result<CreatedAppUserDto>> Handle(CreateAppUserCommand command,
        CancellationToken cancellationToken)
    {
        var user = userRepo.Add(new AppUser(command.UserId)
        {
            Nickname = $"新用户{command.UserId}"
        });

        await userRepo.SaveChangesAsync(cancellationToken);

        return Result.Success(mapper.Map<CreatedAppUserDto>(user));
    }
}