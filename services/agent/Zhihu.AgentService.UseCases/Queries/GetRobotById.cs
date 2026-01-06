using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Zhihu.AgentService.Infrastructure.Contexts;
using Zhihu.Infrastructure.Cache;
using Zhihu.SharedKernel.Messaging;
using Zhihu.SharedKernel.Result;
using Zhihu.AgentService.UseCases;

namespace Zhihu.AgentService.UseCases.Queries;

public record GetRobotById(int Id) : IQuery<Result<RobotDto>>;

public class GetRobotByIdValidator : AbstractValidator<GetRobotById>
{
    public GetRobotByIdValidator()
    {
        RuleFor(command => command.Id).GreaterThan(0);
    }
}

public class GetRobotByIdHandler(
    AgentDbContext dbContext,
    ICacheService<RobotDto> cacheService) : IQueryHandler<GetRobotById, Result<RobotDto>>
{
    public async Task<Result<RobotDto>> Handle(GetRobotById request, CancellationToken cancellationToken)
    {
        var robotDto = await cacheService.GetOrSetByIdAsync(request.Id, async _ =>
        {
            var queryable = dbContext.Robots.AsNoTracking()
                .Where(robot => robot.Id == request.Id)
                .Select(robot => new RobotDto()
                {
                    Id = robot.Id,
                    Name = robot.Name,
                    Knowledge = robot.Knowledge,
                    Character = robot.Character,
                    ExtraPrompt = robot.ExtraPrompt,
                    Temperature = robot.Temperature,
                    Token = robot.Token
                });
            return await queryable.FirstOrDefaultAsync(cancellationToken: cancellationToken);
        });

        return robotDto == null ? Result.NotFound() : Result.Success(robotDto);
    }
}