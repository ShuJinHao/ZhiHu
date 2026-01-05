using Quartz;
using Zhihu.HotService.Infrastructure;
using Zhihu.HotService.UseCases.Managers;

namespace Zhihu.HotService.UseCases.Jobs;

public class UpdateHotRankJob(
    QuestionStatManager questionStatManager,
    HotRankManager hotRankManager,
    IQuestionHttpRepository questionHttpRepository) : IJob
{
    public static JobKey Key = new JobKey(nameof(UpdateHotRankJob), nameof(HotService));

    public async Task Execute(IJobExecutionContext context)
    {
        var questionStats = questionStatManager.GetAndReset();
        if (questionStats == null) return;

        await hotRankManager.UpdateHotRankAsync(questionStats);

        var hotlist = await hotRankManager.GetTopHotRankAsync();
        var ids = hotlist.Select(hot => hot.Id).ToArray();

        var result = await questionHttpRepository.GetQuestionInfoListAsync(ids);

        if (!result.IsSuccess) return;
        
        var questionDict = result.Value!
            .ToDictionary(item => item.Id);

        foreach (var hot in hotlist)
        {
            hot.Title = questionDict[hot.Id].Title;
            hot.Summary = questionDict[hot.Id].Summary;
        }

        await hotRankManager.UpdateHotListAsync(hotlist);
    }
}
