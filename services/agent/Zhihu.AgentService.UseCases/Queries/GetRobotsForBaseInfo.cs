using Microsoft.EntityFrameworkCore;
using Zhihu.AgentService.Infrastructure.Contexts;
using Zhihu.Infrastructure.Cache;
using Zhihu.SharedKernel.Messaging;
using Zhihu.SharedKernel.Result;
using Zhihu.AgentService.UseCases;

namespace Zhihu.AgentService.UseCases.Queries;

public record GetRobotsForBaseInfoQuery(bool IsActive) : IQuery<Result<List<RobotBaseInfoDto>>>;

public class GetRobotsForBaseInfoQueryHandler(
    AgentReadDbContext dbContext,
    ICacheService<RobotBaseInfoDto> cacheService) : IQueryHandler<GetRobotsForBaseInfoQuery, Result<List<RobotBaseInfoDto>>>
{
    public async Task<Result<List<RobotBaseInfoDto>>> Handle(GetRobotsForBaseInfoQuery request, CancellationToken cancellationToken)
    {
        var robots = await cacheService.GetOrSetListAsync(async _ =>
        {
            var queryable = from robot in dbContext.Robots.AsNoTracking()
                where robot.IsActive == request.IsActive
                select new RobotBaseInfoDto
                {
                    Id = robot.Id,
                    Name = robot.Name,
                    Knowledge = robot.Knowledge
                };

            return await queryable.ToListAsync(cancellationToken: _);
        });
        
        if (robots is null || robots.Count == 0)
        {
            return Result.NotFound();
        }
        return Result.Success(robots);
    }
}