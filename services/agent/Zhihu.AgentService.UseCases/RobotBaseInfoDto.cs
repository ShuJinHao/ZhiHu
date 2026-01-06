namespace Zhihu.AgentService.UseCases;

public record RobotBaseInfoDto
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string Knowledge { get; set; }
}
