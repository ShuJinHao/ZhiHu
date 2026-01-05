using Quartz;
using Zhihu.Infrastructure.Quartz;

namespace Zhihu.HotService.UseCases.Jobs;

public class RefreshHotRankJobSchedule : IJobScheduleBuilder
{
    private readonly TriggerKey _key = new(nameof(RefreshHotRankJobSchedule), nameof(HotService));
    private const int Hour = 5;
    
    public IScheduler CreateJobSchedule(IScheduler scheduler)
    {
        var jobDetail = JobBuilder.Create<RefreshHotRankJob>()
            .WithIdentity(RefreshHotRankJob.Key)
            .Build();

        var triggers = new List<ITrigger>
        {
            TriggerBuilder.Create()
                .WithIdentity(_key)
                .ForJob(RefreshHotRankJob.Key)
                .WithCronSchedule($"0 0 {Hour} * * ?")
                .Build(),

            TriggerBuilder.Create()
                .ForJob(RefreshHotRankJob.Key)
                .StartNow()
                .Build(),
        };

        scheduler.ScheduleJob(jobDetail, triggers, true);

        return scheduler;
    }
}
