using FluentValidation;
using Zhihu.Core.Common.Interfaces;
using Zhihu.QuestionService.Core.Data;
using Zhihu.QuestionService.Core.Specifications;
using Zhihu.SharedKernel.Messaging;
using Zhihu.SharedKernel.Result;
using Zhihu.UseCases.Common.Attributes;

namespace Zhihu.QuestionService.UseCases.Answers.Commands;

[Authorize]
public record DeleteAnswerLikeCommand(int QuestionId, int AnswerId) : ICommand<Result>;

public class DeleteAnswerLikeCommandValidator : AbstractValidator<DeleteAnswerLikeCommand>
{
    public DeleteAnswerLikeCommandValidator()
    {
        RuleFor(command => command.QuestionId).GreaterThan(0);
        RuleFor(command => command.AnswerId).GreaterThan(0);
    }
}

public class DeleteAnswerLikeCommandHandler(IAnswerRepository answers, IUser user)
    : ICommandHandler<DeleteAnswerLikeCommand, Result>
{
    public async Task<Result> Handle(DeleteAnswerLikeCommand request, CancellationToken cancellationToken)
    {
        var spec = new AnswerByIdWithLikeByUserIdSpec(request.QuestionId, request.AnswerId, user.Id!.Value);
        var answer = await answers.GetAnswerByIdWithLikeByUserIdAsync(spec, cancellationToken);
        if (answer == null) return Result.NotFound("回答不存在");

        answer.RemoveLike(user.Id!.Value);

        await answers.SaveChangesAsync(cancellationToken);
        
        return Result.Success();
    }
}
