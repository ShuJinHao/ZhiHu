namespace Zhihu.SharedModels.Question;

public record QuestionInfoDto
{
    public int Id { get; set; }
    
    public string Title { get; set; } = null!;

    public string? Summary { get; set; }
}