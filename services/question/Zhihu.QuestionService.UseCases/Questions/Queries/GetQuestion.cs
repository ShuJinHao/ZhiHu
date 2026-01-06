using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Zhihu.Infrastructure.Cache;
using Zhihu.Infrastructure.MessageBus;
using Zhihu.QuestionService.Infrastructure.Contexts;
using Zhihu.SharedKernel.Messaging;
using Zhihu.SharedKernel.Result;
using Zhihu.SharedModels.Question;
using Zhihu.QuestionService.UseCases.Questions;
using Zhihu.QuestionService.UseCases.Questions.Jobs;
using Zhihu.UseCases.Common.Attributes;

namespace Zhihu.QuestionService.UseCases.Questions.Queries;

[Authorize]
public record GetQuestionQuery(int Id) : IQuery<Result<QuestionDto>>;

public class GetQuestionQueryValidator : AbstractValidator<GetQuestionQuery>
{
    public GetQuestionQueryValidator()
    {
        RuleFor(command => command.Id)
            .GreaterThan(0);
    }
}

public class GetQuestionQueryHandler(
    QuestionReadDbContext dbContext,
    ICacheService<QuestionDto> cacheService,
    IMessageBusService bus) : IQueryHandler<GetQuestionQuery, Result<QuestionDto>>
{
    public async Task<Result<QuestionDto>> Handle(GetQuestionQuery request, CancellationToken cancellationToken)
    {
        var questionDto = await cacheService.GetOrSetByIdAsync(request.Id, async _ =>
        {
            var queryable = dbContext.Questions.AsNoTracking()
                .Where(q => q.Id == request.Id)
                .Select(q => new QuestionDto
                {
                    Id = q.Id,
                    Title = q.Title,
                    Description = q.Description,
                    AnswerCount = q.Answers.Count,
                    FollowerCount = q.FollowerCount,
                    ViewCount = q.ViewCount
                });
            return await queryable.FirstOrDefaultAsync(cancellationToken: _);
        });

        if (questionDto == null) return Result.NotFound("问题不存在");
        QuestionViewCounter.Add(questionDto.Id);
        await bus.PublishAsync(new QuestionViewedEvent(questionDto.Id));

        return Result.Success(questionDto);
    }
}