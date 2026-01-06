namespace Zhihu.AgentService.UseCases;

public record RobotDto
{
    public int Id { get; set; }
    
    public required string Name { get; set; }
    
    public required string Knowledge { get; set; }
    
    public string? Character  { get; set; }
    
    public string? ExtraPrompt { get; set; }

    public double Temperature { get; set; }
    
    public string? Token { get; set; }
};