using Zhihu.SharedKernel.Specification;
using Zhihu.QuestionService.Core.Entities;

namespace Zhihu.QuestionService.Core.Specifications;

public class FollowUserByIdSpec : Specification<Question>
{
    public FollowUserByIdSpec(int userId, int questionId)
    {
        FilterCondition = question => question.Id.Equals(questionId);

        AddInclude(user => user.FollowUsers.Where(fq => fq.UserId.Equals(userId)));
    }
}