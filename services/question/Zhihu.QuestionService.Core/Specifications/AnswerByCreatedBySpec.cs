using Zhihu.SharedKernel.Specification;
using Zhihu.QuestionService.Core.Entities;

namespace Zhihu.QuestionService.Core.Specifications;

public class AnswerByCreatedBySpec : Specification<Question>
{
    public AnswerByCreatedBySpec(int userId, int questionId)
    {
        FilterCondition = q => q.Id == questionId && q.CreatedBy == userId;
        AddInclude(q => q.Answers.Take(1));
    }
}