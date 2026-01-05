using Quartz;
using Zhihu.QuestionService.Core.Entities;
using Zhihu.QuestionService.Core.Specifications;
using Zhihu.SharedKernel.Repository;

namespace Zhihu.QuestionService.UseCases.Questions.Jobs;

public class UpdateQuestionViewCountsJob(
    IRepository<Question> questions) : IJob
{
    public static readonly JobKey Key = new(nameof(UpdateQuestionViewCountsJob), nameof(QuestionService));

    private const int BatchSize = 20;

    public async Task Execute(IJobExecutionContext context)
    {
        try
        {
            var questionViewDict = QuestionViewCounter.GetAndReset();
            if (questionViewDict == null) return;

            var totalBatch = (int)Math.Ceiling((double)questionViewDict.Count / BatchSize);

            for (var i = 1; i <= totalBatch; i++)
            {
                var batchData = questionViewDict.Skip((i - 1) * totalBatch).Take(BatchSize).ToDictionary();
                var ids = batchData.Keys.ToArray();
                var questionEntities = await questions.GetListAsync(new QuestionsByIdsSpec(ids));

                if (questionEntities.Count == 0) return;

                foreach (var questionEntity in questionEntities) questionEntity.AddViewCount(batchData[questionEntity.Id]);

                await questions.SaveChangesAsync();
            }
        }
        catch (Exception ex)
        {
            throw new JobExecutionException(ex.Message, ex, true);
        }
    }
}