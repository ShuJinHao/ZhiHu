namespace Zhihu.SharedModels.Feed;

public record FeedDto
{
    public int FeedId { get; set; }

    public FeedType FeedType { get; set; }

    public int SenderId { get; set; }
};
