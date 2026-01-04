using FluentValidation;
using Zhihu.Core.Common.Interfaces;
using Zhihu.QuestionService.Core.Entities;
using Zhihu.QuestionService.Core.Specifications;
using Zhihu.SharedKernel.Messaging;
using Zhihu.SharedKernel.Repository;
using Zhihu.SharedKernel.Result;
using Zhihu.UseCases.Common.Attributes;

namespace Zhihu.QuestionService.UseCases.Answers.Commands;

[Authorize]
public record DeleteAnswerCommand(int QuestionId, int AnswerId) : ICommand<Result>;

public class DeleteAnswerCommandValidator : AbstractValidator<DeleteAnswerCommand>
{
    public DeleteAnswerCommandValidator()
    {
        RuleFor(command => command.QuestionId)
            .GreaterThan(0);

        RuleFor(command => command.AnswerId)
            .GreaterThan(0);
    }
}

public class DeleteAnswerCommandHandler(
    IRepository<Question> questions,
    IUser user) : ICommandHandler<DeleteAnswerCommand, Result>
{
    public async Task<Result> Handle(DeleteAnswerCommand request, CancellationToken cancellationToken)
    {
        var spec = new AnswerByIdAndCreatedBySpec(user.Id!.Value, request.QuestionId, request.AnswerId);
        
        var question = await questions.GetSingleOrDefaultAsync(spec, cancellationToken);
        if (question == null) return Result.NotFound("问题不存在");

        var answer = question.Answers.FirstOrDefault(a => a.Id == request.AnswerId);
        if (answer == null) return Result.NotFound("回答不存在");

        question.Answers.Remove(answer);

        await questions.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
