namespace Zhihu.UserService.UseCases;

public record UserInfoDto
{
    public int Id { get; set; }

    public string? Nickname { get; set; }

    public string? Avatar { get; set; }

    public string? Bio { get; set; }

    public int FolloweesCount { get; set; }

    public int FollowersCount { get; set; }
}