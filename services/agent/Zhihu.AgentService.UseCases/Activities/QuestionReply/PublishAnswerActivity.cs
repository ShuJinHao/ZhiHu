using Dapr.Workflow;
using Zhihu.AgentService.Infrastructure.Repositories;

namespace Zhihu.AgentService.UseCases.Activities.QuestionReply;

public class PublishAnswerActivity(IQuestionHttpRepository questionHttpRepository) :
    WorkflowActivity<(int questionId, string answer, RobotDto robot), bool>
{
    public override async Task<bool> RunAsync(WorkflowActivityContext context, (int questionId, string answer, RobotDto robot) input)
    {
        var result = await questionHttpRepository.CreateAnswerAsync(input.questionId, input.answer, input.robot.Token!);
        return result.IsSuccess;
    }
}