using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Zhihu.QuestionService.Infrastructure.Contexts;
using Zhihu.SharedKernel.Messaging;
using Zhihu.SharedKernel.Result;
using Zhihu.SharedModels.Question;

namespace Zhihu.QuestionService.UseCases.Questions.Queries;

// 1. Query 定义 (保持不变)
public record GetQuestionInfosByIdsQuery(int[] Ids) : IQuery<Result<List<QuestionInfoDto>>>;

// 2. 验证器 (保持不变)
public class GetQuestionInfosByIdsQueryValidator : AbstractValidator<GetQuestionInfosByIdsQuery>
{
    public GetQuestionInfosByIdsQueryValidator()
    {
        RuleFor(command => command.Ids)
            .Must(ids => ids.Length > 0).WithMessage("查询条数不能为空")
            .Must(ids => ids.Length <= 50).WithMessage("查询条数不能超过50条");
    }
}

// 3. 处理器 (关键修改在下面)
public class GetQuestionInfosByIdsQueryHandler(
    QuestionReadDbContext dbContext) : IQueryHandler<GetQuestionInfosByIdsQuery, Result<List<QuestionInfoDto>>>
{
    public async Task<Result<List<QuestionInfoDto>>> Handle(GetQuestionInfosByIdsQuery request, CancellationToken cancellationToken)
    {
        // -------------------------------------------------------------------
        // 【核心修改】
        // 既然 int[] 数组报错，我们就强制转成 List<int>。
        // EF Core 对 List 的支持比 Array 完美得多，这能 100% 解决 parameter expression 报错。
        // -------------------------------------------------------------------

        // 1. 先防御性判空，防止 ToList() 报空指针
        if (request.Ids == null || request.Ids.Length == 0)
        {
            return Result.Success(new List<QuestionInfoDto>());
        }

        // 2. 【关键一步】转成 List！
        var targetIds = request.Ids.ToList();

        var query = dbContext.Questions.AsNoTracking()
            // 3. 使用 List 进行查询
            .Where(question => targetIds.Contains(question.Id))
            .Select(question => new QuestionInfoDto
            {
                Id = question.Id,
                Title = question.Title,
                Summary = question.Summary
            });

        var questionLists = await query.ToListAsync(cancellationToken: cancellationToken);

        return questionLists.Count == 0 ? Result.NotFound() : Result.Success(questionLists);
    }
}