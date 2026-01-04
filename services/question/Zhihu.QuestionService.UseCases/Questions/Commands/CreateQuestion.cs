using AutoMapper;
using FluentValidation;
using Zhihu.Core.Common.Interfaces;
using Zhihu.Infrastructure.MessageBus;
using Zhihu.QuestionService.Core.Data;
using Zhihu.QuestionService.Core.Entities;
using Zhihu.SharedKernel.Messaging;
using Zhihu.SharedKernel.Repository;
using Zhihu.SharedKernel.Result;
using Zhihu.SharedModels.Feed;
using Zhihu.UseCases.Common.Attributes;
using Zhihu.QuestionService.UseCases.Questions;

namespace Zhihu.QuestionService.UseCases.Questions.Commands;

[Authorize]
public record CreateQuestionCommand(string Title, string? Description) : ICommand<Result<CreatedQuestionDto>>;

public class CreateQuestionCommandValidator : AbstractValidator<CreateQuestionCommand>
{
    public CreateQuestionCommandValidator()
    {
        RuleFor(command => command.Title)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .Length(6, DataSchemaConstants.DefaultQuestionTitleLength)
            .Must(t => t.EndsWith('?') || t.EndsWith('？')).WithMessage("问题标题必须以问号结尾");

        RuleFor(command => command.Description)
            .MaximumLength(DataSchemaConstants.DefaultDescriptionTitleLength);
    }
}

public class CreateQuestionCommandHandler(
    IUser user,
    IRepository<Question> questions,
    IMapper mapper,
    IMessageBusService bus) : ICommandHandler<CreateQuestionCommand, Result<CreatedQuestionDto>>
{
    public async Task<Result<CreatedQuestionDto>> Handle(CreateQuestionCommand request,
        CancellationToken cancellationToken)
    {
        var question = mapper.Map<Question>(request);

        question.GenerateSummary();

        questions.Add(question);

        await questions.SaveChangesAsync(cancellationToken);

        await bus.PublishAsync(new FeedCreatedEvent
        {
            FeedType = FeedType.Quesiton,
            FeedId = question.Id,
            UserId = user.Id!.Value
        });

        return Result.Success(new CreatedQuestionDto(question.Id));
    }
}
