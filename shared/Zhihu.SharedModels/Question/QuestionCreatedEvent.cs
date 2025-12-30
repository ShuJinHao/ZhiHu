namespace Zhihu.SharedModels.Question;

public class QuestionCreatedEvent
{
    public int Id { get; set; }
    public required string Title { get; set; }
    public string? Description { get; set; }
}