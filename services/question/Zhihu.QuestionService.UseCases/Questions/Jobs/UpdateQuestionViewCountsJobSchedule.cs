using Quartz;
using Zhihu.Infrastructure.Quartz;

namespace Zhihu.QuestionService.UseCases.Questions.Jobs;

public class UpdateQuestionViewCountsJobSchedule : IJobScheduleBuilder
{
    private static readonly TriggerKey Key = new(nameof(UpdateQuestionViewCountsJobSchedule), nameof(Questions));
    private const int IntervalTime = 10;
    
    public IScheduler CreateJobSchedule(IScheduler scheduler)
    {
        var jobDetail = JobBuilder.Create<UpdateQuestionViewCountsJob>()
            .WithIdentity(UpdateQuestionViewCountsJob.Key)
            .Build();

        var trigger = TriggerBuilder.Create()
            .WithIdentity(Key)
            .ForJob(UpdateQuestionViewCountsJob.Key)
            .StartAt(DateBuilder.FutureDate(IntervalTime, IntervalUnit.Second))
            .WithSimpleSchedule(builder => builder
                .RepeatForever()
                .WithInterval(TimeSpan.FromSeconds(IntervalTime)))
            .Build();

        scheduler.ScheduleJob(jobDetail, trigger);
        
        return scheduler;
    }
}
