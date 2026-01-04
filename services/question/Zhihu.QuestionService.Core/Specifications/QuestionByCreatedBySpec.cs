using Zhihu.SharedKernel.Specification;
using Zhihu.QuestionService.Core.Entities;

namespace Zhihu.QuestionService.Core.Specifications;

public class QuestionByCreatedBySpec : Specification<Question>
{
    public QuestionByCreatedBySpec(int userId, int questionId)
    {
        FilterCondition = q => q.Id == questionId && q.CreatedBy == userId;
    }
}