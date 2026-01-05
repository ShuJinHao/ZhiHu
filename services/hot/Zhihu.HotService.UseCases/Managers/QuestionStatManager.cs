using Zhihu.HotService.Core.Entities;

namespace Zhihu.HotService.UseCases.Managers;

public class QuestionStatManager
{
    private readonly object _lock = new();

    private Dictionary<int, QuestionStat> Items { get; set; } = new();

    public void Set(Dictionary<int, QuestionStat> items)
    {
        lock (_lock)
        {
            Items = items;
            Reset();
        }
    }

    public void AddViewCount(int id, int count = 1)
    {
        lock (_lock)
        {
            if(!Items.TryGetValue(id, out var stat)) return;
            stat.ViewCount += count;
        }
    }

    public void AddAnswerCount(int id, int count = 1)
    {
        lock (_lock)
        {
            if(!Items.TryGetValue(id, out var stat)) return;
            stat.AnswerCount += count;
        }
    }

    public void AddLikeCount(int id, int count = 1)
    {
        lock (_lock)
        {
            if(!Items.TryGetValue(id, out var stat)) return;
            stat.LikeCount += count;
        }
    }

    public void AddFollowCount(int id, int count = 1)
    {
        lock (_lock)
        {
            if(!Items.TryGetValue(id, out var stat)) return;
            stat.FollowCount += count;
        }
    }

    public Dictionary<int, QuestionStat>? GetAndReset()
    {
        if (Items.Count == 0) return null;

        lock (_lock)
        {
            var result = Items.ToDictionary(
                kvp => kvp.Key,
                kvp => kvp.Value with { });
            Reset();
            return result;
        }
    }

    private void Reset()
    {
        foreach (var item in Items)
        {
            item.Value.ViewCount = 0;
            item.Value.LikeCount = 0;
            item.Value.AnswerCount = 0;
            item.Value.FollowCount = 0;
        }
    }

}
