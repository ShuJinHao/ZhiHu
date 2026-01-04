using Zhihu.SharedKernel.Specification;
using Zhihu.QuestionService.Core.Entities;

namespace Zhihu.QuestionService.Core.Specifications;

public class QuestionByIdSpec : Specification<Question>
{
    public QuestionByIdSpec(int id)
    {
        FilterCondition = question => question.Id == id;
    }
}