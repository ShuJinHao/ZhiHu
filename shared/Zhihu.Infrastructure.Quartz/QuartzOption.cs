using System.Collections.Specialized;

namespace Zhihu.Infrastructure.Quartz;

public class QuartzOption
{
    public SchedulerOption[] Schedulers { get; init; } = null!;
}

public class SchedulerOption : Dictionary<string, string?>
{
    public NameValueCollection ToNameValueCollection()
    {
        var collection = new NameValueCollection(Count);
        foreach (var pair in this)
        {
            collection[pair.Key] = pair.Value;
        }

        return collection;
    }
}
