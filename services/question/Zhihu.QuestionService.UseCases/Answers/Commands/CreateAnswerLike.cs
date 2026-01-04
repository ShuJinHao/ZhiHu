using FluentValidation;
using Zhihu.Core.Common.Interfaces;
using Zhihu.QuestionService.Core.Data;
using Zhihu.QuestionService.Core.Specifications;
using Zhihu.SharedKernel.Messaging;
using Zhihu.SharedKernel.Result;
using Zhihu.UseCases.Common.Attributes;

namespace Zhihu.QuestionService.UseCases.Answers.Commands;

[Authorize]
public record CreateAnswerLikeCommand(int QuestionId, int AnswerId, bool IsLike) : ICommand<Result>;

public class CreateAnswerLikeCommandValidator : AbstractValidator<CreateAnswerLikeCommand>
{
    public CreateAnswerLikeCommandValidator()
    {
        RuleFor(command => command.QuestionId).GreaterThan(0);
        RuleFor(command => command.AnswerId).GreaterThan(0);
    }
}

public class CreateAnswerLikeCommandHandler(IAnswerRepository answers, IUser user)
    : ICommandHandler<CreateAnswerLikeCommand, Result>
{
    public async Task<Result> Handle(CreateAnswerLikeCommand request, CancellationToken cancellationToken)
    {
        var spec = new AnswerByIdWithLikeByUserIdSpec(request.QuestionId, request.AnswerId, user.Id!.Value);
        var answer = await answers.GetAnswerByIdWithLikeByUserIdAsync(spec, cancellationToken);
        if (answer == null) return Result.NotFound("回答不存在");

        var result = answer.AddLike(user.Id!.Value, request.IsLike);

        if (!result.IsSuccess) return result;

        await answers.SaveChangesAsync(cancellationToken);
        
        return Result.Success();
    }
}
