using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Zhihu.Infrastructure.Cache;
using Zhihu.Infrastructure.EFCore;
using Zhihu.QuestionService.Infrastructure.Contexts;
using Zhihu.SharedKernel.Messaging;
using Zhihu.SharedKernel.Paging;
using Zhihu.SharedKernel.Result;
using Zhihu.QuestionService.UseCases.Answers;

namespace Zhihu.QuestionService.UseCases.Answers.Queries;

public record GetAnswersQuery(int QuestionId, Pagination Pagination) : IQuery<Result<PagedList<AnswerDto>>>;

public class GetQuestionQueryValidator : AbstractValidator<GetAnswersQuery>
{
    public GetQuestionQueryValidator()
    {
        RuleFor(command => command.QuestionId)
            .GreaterThan(0);
    }
}

public class GetAnswersQueryHandler(
    QuestionDbContext dbContext,
    ICacheService<AnswerDto> cacheService) : IQueryHandler<GetAnswersQuery, Result<PagedList<AnswerDto>>>
{
    public async Task<Result<PagedList<AnswerDto>>> Handle(GetAnswersQuery request, CancellationToken cancellationToken)
    {
        var answers = await cacheService.GetOrSetListByPageAsync(request.QuestionId, request.Pagination, async _ =>
        {
            var queryable = from answer in dbContext.Answers.AsNoTracking()
                            where answer.QuestionId == request.QuestionId
                            orderby answer.LikeCount descending
                            select new AnswerDto
                            {
                                Id = answer.Id,
                                Content = answer.Content,
                                LikeCount = answer.LikeCount,
                                LastModifiedAt = answer.LastModifiedAt,
                                CreatedBy = answer.CreatedBy,
                                CreatedByType = answer.CreatedByType
                            };

            return await queryable.ToPageListAsync(request.Pagination);
        });

        return answers == null ? Result.NotFound("回答不存在") : Result.Success(answers);
    }
}