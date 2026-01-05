namespace Zhihu.SharedModels.Question;

public record QuestionStatDto
{
    public int Id { get; set; }
    
    public int ViewCount { get; set; }
    
    public int FollowCount { get; set; }
    
    public int AnswerCount { get; set; }
    
    public int LikeCount { get; set; }
}