using Zhihu.SharedKernel.Specification;
using Zhihu.QuestionService.Core.Entities;

namespace Zhihu.QuestionService.Core.Specifications;

public class QuestionsByIdsSpec : Specification<Question>
{
    public QuestionsByIdsSpec(int[] ids)
    {
        FilterCondition = question => ids.Contains(question.Id);
    }
}