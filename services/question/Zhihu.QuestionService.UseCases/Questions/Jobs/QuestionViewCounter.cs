namespace Zhihu.QuestionService.UseCases.Questions.Jobs;

public static class QuestionViewCounter
{
    private static readonly Lock Lock = new();
    private static Dictionary<int, int> Item { get; } = new();

    public static void Add(int id, int count = 1)
    {
        lock (Lock)
        {
            if (!Item.TryAdd(id, count)) Item[id] += count;
        }
    }

    public static Dictionary<int, int>? GetAndReset()
    {
        if (Item.Count == 0) return null;
        lock (Lock)
        {
            var result = new Dictionary<int, int>(Item);
            Item.Clear();
            return result;
        }
    }
}
