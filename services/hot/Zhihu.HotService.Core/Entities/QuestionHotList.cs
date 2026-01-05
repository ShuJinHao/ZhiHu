namespace Zhihu.HotService.Core.Entities;

public record QuestionHotList
{
    public int Id { get; set; }

    public int HotValue { get; set; }

    public string Title { get; set; } = null!;

    public string? Summary { get; set; }
}
