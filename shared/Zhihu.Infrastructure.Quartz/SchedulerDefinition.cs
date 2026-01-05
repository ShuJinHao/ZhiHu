namespace Zhihu.Infrastructure.Quartz;

public static class SchedulerDefinition
{
    public static readonly string ClusteredScheduler = nameof(ClusteredScheduler);
    public static readonly string LocalScheduler = nameof(LocalScheduler);
}