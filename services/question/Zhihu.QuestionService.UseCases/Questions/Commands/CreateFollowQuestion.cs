using FluentValidation;
using Zhihu.Core.Common.Interfaces;
using Zhihu.Infrastructure.MessageBus;
using Zhihu.QuestionService.Core.Entities;
using Zhihu.QuestionService.Core.Specifications;
using Zhihu.SharedKernel.Messaging;
using Zhihu.SharedKernel.Repository;
using Zhihu.SharedKernel.Result;
using Zhihu.SharedModels.Question;
using Zhihu.UseCases.Common.Attributes;

namespace Zhihu.QuestionService.UseCases.Questions.Commands;

[Authorize]
public record CreateFollowCommand(int QuestionId) : ICommand<Result>;

public class CreateFollowCommandValidator : AbstractValidator<CreateFollowCommand>
{
    public CreateFollowCommandValidator()
    {
        RuleFor(command => command.QuestionId)
            .GreaterThan(0);
    }
}

public class CreateFollowCommandHandler(
    IRepository<Question> questions,
    IUser user,
    IMessageBusService bus) : ICommandHandler<CreateFollowCommand, Result>
{
    public async Task<Result> Handle(CreateFollowCommand request, CancellationToken cancellationToken)
    {
        var spec = new FollowUserByIdSpec(user.Id!.Value, request.QuestionId);
        var question = await questions.GetSingleOrDefaultAsync(spec, cancellationToken);
        if (question == null) return Result.NotFound("问题不存在");

        var result = question.AddFollowQuestion(user.Id!.Value);
        if (!result.IsSuccess) return result;

        await questions.SaveChangesAsync(cancellationToken);

        await bus.PublishAsync(new FollowQuestionAddedEvent
        {
            QuestionId = question.Id
        });
        
        return Result.Success();
    }
}