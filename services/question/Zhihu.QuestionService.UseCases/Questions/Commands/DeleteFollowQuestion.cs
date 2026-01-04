using FluentValidation;
using Zhihu.QuestionService.Core.Entities;
using Zhihu.QuestionService.Core.Specifications;
using Zhihu.SharedKernel.Messaging;
using Zhihu.SharedKernel.Repository;
using Zhihu.SharedKernel.Result;
using Zhihu.UseCases.Common.Attributes;

namespace Zhihu.QuestionService.UseCases.Questions.Commands;

[Authorize]
public record DeleteFollowCommand(int QuestionId, int UserId) : ICommand<Result>;

public class DeleteFollowCommandValidator : AbstractValidator<DeleteFollowCommand>
{
    public DeleteFollowCommandValidator()
    {
        RuleFor(command => command.QuestionId)
            .GreaterThan(0);
    }
}

public class DeleteFollowCommandHanlder(
    IRepository<Question> questions) : ICommandHandler<DeleteFollowCommand, Result>
{
    public async Task<Result> Handle(DeleteFollowCommand request, CancellationToken cancellationToken)
    {
        var spec = new FollowUserByIdSpec(request.UserId, request.QuestionId);
        var question = await questions.GetSingleOrDefaultAsync(spec, cancellationToken);
        if (question == null) return Result.NotFound("问题不存在");

        question.RemoveFollowQuestion(request.UserId);

        await questions.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}