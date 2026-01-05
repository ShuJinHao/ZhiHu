using Quartz;
using Quartz.Impl.Matchers;
using Zhihu.HotService.Core.Entities;
using Zhihu.HotService.Infrastructure;
using Zhihu.HotService.UseCases.Managers;

namespace Zhihu.HotService.UseCases.Jobs;

public class RefreshHotRankJob(
    IQuestionHttpRepository questionHttpRepository,
    QuestionStatManager questionStatManager,
    HotRankManager hotRankManager) : IJob
{
    public static readonly JobKey Key = new(nameof(RefreshHotRankJob), nameof(HotService));

    public async Task Execute(IJobExecutionContext context)
    {
        var createdAtBegin = DateTimeOffset.Now.AddDays(-7);
        var lastModifiedBegin = DateTimeOffset.Now.AddHours(-48);
        
        var result = await questionHttpRepository.GetLatestQuestionStatListAsync(createdAtBegin, lastModifiedBegin);
        if (!result.IsSuccess) return;

        var questionStats = result.Value!
            .ToDictionary(item => item.Id, item => new QuestionStat
            {
                ViewCount = item.ViewCount,
                FollowCount = item.FollowCount,
                AnswerCount = item.AnswerCount,
                LikeCount = item.LikeCount
            });

        var triggerKey = GroupMatcher<TriggerKey>.GroupEquals(nameof(UpdateHotRankJob));
        await context.Scheduler.PauseTriggers(triggerKey);

        await hotRankManager.ClearHotRankAsync();
        await hotRankManager.CreateHotRankAsync(questionStats);

        questionStatManager.Set(questionStats);

        await context.Scheduler.ResumeTriggers(triggerKey);
    }
}
