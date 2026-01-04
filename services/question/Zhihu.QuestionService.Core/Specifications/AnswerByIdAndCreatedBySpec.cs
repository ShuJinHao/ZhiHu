using Zhihu.SharedKernel.Specification;
using Zhihu.QuestionService.Core.Entities;

namespace Zhihu.QuestionService.Core.Specifications;

public class AnswerByIdAndCreatedBySpec : Specification<Question>
{
    public AnswerByIdAndCreatedBySpec(int userId, int questionId, int answerId)
    {
        FilterCondition = q => q.Id == questionId;
        AddInclude(q => q.Answers.Where(a => a.Id == answerId && a.CreatedBy == userId));
    }
}