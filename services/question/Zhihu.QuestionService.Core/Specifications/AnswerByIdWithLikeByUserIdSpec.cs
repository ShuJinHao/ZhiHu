using Zhihu.SharedKernel.Specification;
using Zhihu.QuestionService.Core.Entities;

namespace Zhihu.QuestionService.Core.Specifications;

public class AnswerByIdWithLikeByUserIdSpec : Specification<Answer>
{
    public AnswerByIdWithLikeByUserIdSpec(int questionId, int answerId, int userId)
    {
        FilterCondition = answer => answer.QuestionId == questionId && answer.Id == answerId;
        AddInclude(answer => answer.AnswerLikes.Where(al => al.UserId == userId));
    }
}