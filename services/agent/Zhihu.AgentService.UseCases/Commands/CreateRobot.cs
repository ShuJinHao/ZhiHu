using AutoMapper;
using FluentValidation;
using Zhihu.AgentService.Core.Entities;
using Zhihu.AgentService.Infrastructure.Tools;
using Zhihu.SharedKernel.Messaging;
using Zhihu.SharedKernel.Repository;
using Zhihu.SharedKernel.Result;

namespace Zhihu.AgentService.UseCases.Commands;

public record CreateRobotCommand(string Name, string Knowledge, double Temperature = 0.7) : ICommand<Result>
{
    public string? Character { get; set; }

    public string? ExtraPrompt { get; set; }
}

public class CreateRobotCommandValidator : AbstractValidator<CreateRobotCommand>
{
    public CreateRobotCommandValidator()
    {
        RuleFor(command => command.Name).NotEmpty().NotNull();
        RuleFor(command => command.Knowledge).NotEmpty().NotNull();
        RuleFor(command => command.Temperature).GreaterThan(0);
    }
}

public class CreateRobotCommandHandler(
    IRepository<Robot> robotRepo,
    RobotJwtGenerator robotJwtGenerator,
    IMapper mapper) : ICommandHandler<CreateRobotCommand, Result>
{
    public async Task<Result> Handle(CreateRobotCommand command,
        CancellationToken cancellationToken)
    {
        var robot = mapper.Map<Robot>(command);
        robot.IsActive = true;
        robotRepo.Add(robot);
        await robotRepo.SaveChangesAsync(cancellationToken);

        robot.Token = robotJwtGenerator.GetAccessToken(robot.Id, robot.Name);
        await robotRepo.SaveChangesAsync(cancellationToken);
        
        return Result.Success();
    }
}