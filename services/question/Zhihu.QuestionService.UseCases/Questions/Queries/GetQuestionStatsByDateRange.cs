using Microsoft.EntityFrameworkCore;
using Zhihu.QuestionService.Infrastructure.Contexts;
using Zhihu.SharedKernel.Messaging;
using Zhihu.SharedKernel.Result;
using Zhihu.SharedModels.Question;
using Zhihu.UseCases.Common.Attributes;

namespace Zhihu.QuestionService.UseCases.Questions.Queries;

[Authorize]
public record GetQuestionStatsByDateQuery(DateTimeOffset CreatedAtBegin, DateTimeOffset LastModifiedBegin) : IQuery<Result<List<QuestionStatDto>>>;

public class GetQuestionStatsByDateQueryHandler(
    QuestionReadDbContext dbContext) : IQueryHandler<GetQuestionStatsByDateQuery, Result<List<QuestionStatDto>>>
{
    public async Task<Result<List<QuestionStatDto>>> Handle(GetQuestionStatsByDateQuery request, CancellationToken cancellationToken)
    {
        var questions = dbContext.Questions.AsNoTracking();
        var answers = dbContext.Answers.AsNoTracking();

        var query = from question in questions
                    join answer in answers on question.Id equals answer.QuestionId
                    group answer by new
                    {
                        question.Id,
                        question.ViewCount,
                        question.FollowerCount,
                        question.LastModifiedAt,
                        question.CreatedAt
                    } into g
                    where g.Key.LastModifiedAt >= request.LastModifiedBegin
                          && g.Key.CreatedAt >= request.CreatedAtBegin
                    orderby g.Key.ViewCount descending
                    select new QuestionStatDto
                    {
                        Id = g.Key.Id,
                        ViewCount = g.Key.ViewCount,
                        FollowCount = g.Key.FollowerCount,
                        AnswerCount = g.Count(),
                        LikeCount = g.Sum(answer => answer.LikeCount)
                    };
        var questionStatList = await query.ToListAsync(cancellationToken: cancellationToken);

        return questionStatList.Count == 0 ? Result.NotFound() : Result.Success(questionStatList);
    }
}