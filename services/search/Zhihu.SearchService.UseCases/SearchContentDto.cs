using System.Text.Json.Serialization;

namespace Zhihu.SearchService.UseCases;

public record ContentDto
{
    public int Id { get; set; }

    [JsonPropertyName("questionid")]
    public int? QuestionId { get; set; }

    [JsonPropertyName("likecount")]
    public int? LikeCount { get; set; }
}
