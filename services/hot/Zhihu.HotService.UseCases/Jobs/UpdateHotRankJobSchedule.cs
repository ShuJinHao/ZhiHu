using Quartz;
using Zhihu.Infrastructure.Quartz;

namespace Zhihu.HotService.UseCases.Jobs;

public class UpdateHotRankJobSchedule : IJobScheduleBuilder
{
    private const int IntervalTime = 60;
    
    public IScheduler CreateJobSchedule(IScheduler scheduler)
    {
        var schedulerId = scheduler.SchedulerInstanceId;
        var triggerKey = new TriggerKey($"{nameof(UpdateHotRankJobSchedule)}-{schedulerId}", nameof(UpdateHotRankJob));

        var jobDetail = JobBuilder.Create<UpdateHotRankJob>()
            .WithIdentity(UpdateHotRankJob.Key)
            .Build();

        var trigger = TriggerBuilder.Create()
            .WithIdentity(triggerKey)
            .ForJob(UpdateHotRankJob.Key)
            .WithSimpleSchedule(builder => builder
                .RepeatForever()
                .WithInterval(TimeSpan.FromSeconds(IntervalTime)))
            .Build();

        scheduler.ScheduleJob(jobDetail, trigger);
        return scheduler;
    }
}
