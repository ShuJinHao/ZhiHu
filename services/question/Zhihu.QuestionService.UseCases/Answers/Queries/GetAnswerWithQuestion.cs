using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Zhihu.Infrastructure.Cache;
using Zhihu.QuestionService.Infrastructure.Contexts;
using Zhihu.QuestionService.UseCases.Questions;
using Zhihu.SharedKernel.Messaging;
using Zhihu.SharedKernel.Result;
using Zhihu.QuestionService.UseCases.Answers;

namespace Zhihu.QuestionService.UseCases.Answers.Queries;

public record GetAnswerWithQuestionQuery(int QuestionId, int AnswerId) : IQuery<Result<AnswerWithQuestionDto>>;

public class GetAnswerWithQuestionQueryValidator : AbstractValidator<GetAnswerWithQuestionQuery>
{
    public GetAnswerWithQuestionQueryValidator()
    {
        RuleFor(command => command.QuestionId)
            .GreaterThan(0);

        RuleFor(command => command.AnswerId)
            .GreaterThan(0);
    }
}

public class GetAnswerQueryHandler(
    QuestionDbContext dbContext,
    ICacheService<AnswerWithQuestionDto> cacheService) : IQueryHandler<GetAnswerWithQuestionQuery, Result<AnswerWithQuestionDto>>
{
    public async Task<Result<AnswerWithQuestionDto>> Handle(
        GetAnswerWithQuestionQuery request,
        CancellationToken cancellationToken)
    {

        var answerDto = await cacheService.GetOrSetByIdAsync(request.QuestionId, request.AnswerId, async _ =>
        {
            var answers = dbContext.Answers.AsNoTracking();
            var questions = dbContext.Questions.AsNoTracking();

            var queryable = from answer in answers
                join question in questions on answer.QuestionId equals question.Id
                select new AnswerWithQuestionDto
                {
                    Answer = new AnswerDto
                    {
                        Id = answer.Id,
                        Content = answer.Content,
                        LikeCount = answer.LikeCount,
                        LastModifiedAt = answer.LastModifiedAt,
                        CreatedBy = answer.CreatedBy
                    },
                    Question = new QuestionDto
                    {
                        Id = question.Id,
                        Title = question.Title,
                        Description = question.Description,
                        AnswerCount = question.Answers.Count,
                        FollowerCount = question.FollowerCount,
                        ViewCount = question.ViewCount
                    }
                };

            return await queryable.FirstOrDefaultAsync(cancellationToken: _);
        });

        return answerDto == null ? Result.NotFound("回答不存在") : Result.Success(answerDto);
    }
}
