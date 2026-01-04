using AutoMapper;
using FluentValidation;
using Zhihu.Core.Common.Interfaces;
using Zhihu.QuestionService.Core.Data;
using Zhihu.QuestionService.Core.Entities;
using Zhihu.QuestionService.Core.Specifications;
using Zhihu.SharedKernel.Messaging;
using Zhihu.SharedKernel.Repository;
using Zhihu.SharedKernel.Result;
using Zhihu.UseCases.Common.Attributes;

namespace Zhihu.QuestionService.UseCases.Questions.Commands;

[Authorize]
public record UpdateQuestionCommand(int Id, string Title, string? Description) : ICommand<IResult>;

public class UpdateQuestionCommandValidator : AbstractValidator<UpdateQuestionCommand>
{
    public UpdateQuestionCommandValidator()
    {
        RuleFor(command => command.Id)
            .GreaterThan(0);

        RuleFor(command => command.Title)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .Length(6, DataSchemaConstants.DefaultQuestionTitleLength)
            .Must(t => t.EndsWith('?') || t.EndsWith('？')).WithMessage("问题标题必须以问号结尾");

        RuleFor(command => command.Description)
            .MaximumLength(DataSchemaConstants.DefaultDescriptionTitleLength);
    }
}

public class UpdateQuestionCommandHandler(
    IRepository<Question> questions,
    IUser user,
    IMapper mapper) : ICommandHandler<UpdateQuestionCommand, IResult>
{
    public async Task<IResult> Handle(UpdateQuestionCommand request, CancellationToken cancellationToken)
    {
        var spec = new QuestionByCreatedBySpec(user.Id!.Value, request.Id);
        var question = await questions.GetSingleOrDefaultAsync(spec, cancellationToken);
        if (question == null) return Result.NotFound("问题不存在");

        mapper.Map(request, question);

        question.GenerateSummary();

        await questions.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
