using AutoMapper;
using FluentValidation;
using Zhihu.Core.Common.Interfaces;
using Zhihu.Infrastructure.MessageBus;
using Zhihu.QuestionService.Core.Entities;
using Zhihu.SharedKernel.Messaging;
using Zhihu.SharedKernel.Repository;
using Zhihu.SharedKernel.Result;
using Zhihu.SharedModels.Feed;
using Zhihu.SharedModels.Question;
using Zhihu.UseCases.Common.Attributes;
using Zhihu.QuestionService.UseCases.Answers;

namespace Zhihu.QuestionService.UseCases.Answers.Commands;

[Authorize]
public record CreateAnswerCommand(int QuestionId, string Content) : ICommand<Result<CreatedAnswerDto>>;

public class CreateAnswerCommandValidator : AbstractValidator<CreateAnswerCommand>
{
    public CreateAnswerCommandValidator()
    {
        RuleFor(command => command.QuestionId)
            .GreaterThan(0);

        RuleFor(command => command.Content)
            .NotEmpty();
    }
}

public class CreateAnswerCommandHandler(
    IUser user,
    IRepository<Question> questions,
    IMapper mapper,
    IMessageBusService bus) : ICommandHandler<CreateAnswerCommand, Result<CreatedAnswerDto>>
{
    public async Task<Result<CreatedAnswerDto>> Handle(CreateAnswerCommand request, CancellationToken cancellationToken)
    {
        var question = await questions.GetByIdAsync(request.QuestionId, cancellationToken);
        if (question == null) return Result.NotFound("问题不存在");

        var answer = mapper.Map<Answer>(request);

        question.Answers.Add(answer);

        answer.AddDomainEvent(new AnswerCreatedEvent(question.Id));

        await questions.SaveChangesAsync(cancellationToken);

        await bus.PublishAsync(new FeedCreatedEvent
        {
            FeedType = FeedType.Answer,
            FeedId = answer.Id,
            UserId = user.Id!.Value
        });

        return Result.Success(new CreatedAnswerDto(question.Id, answer.Id));
    }
}