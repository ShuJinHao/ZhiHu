using Dapr.Workflow;
using Zhihu.SharedModels.Question;
using Zhihu.AgentService.UseCases;
using Zhihu.AgentService.UseCases.Activities.QuestionReply;

namespace Zhihu.AgentService.UseCases.Workflows;

public class QuestionReplyingWorkflow : Workflow<QuestionCreatedEvent, WorkflowResult>
{
    public override async Task<WorkflowResult> RunAsync(WorkflowContext context, QuestionCreatedEvent question)
    {
        var questionContent = $"{question.Title}\r\n{question.Description}";
        
        try
        {
            // 问题知识领域识别（选择机器人）
            var robot =
                await context.CallActivityAsync<RobotDto?>(nameof(RobotSelectActivity), questionContent);
            if (robot is null) return new WorkflowResult("机器人选择失败", false);
            
            // 获取回答
            var answer =
                await context.CallActivityAsync<string?>(nameof(AnswerGenerateActivity), (questionContent, robot));
            if (string.IsNullOrEmpty(answer)) return new WorkflowResult("回答内容为空", false); 
            
            // 发布回答
            var pubResult =
                await context.CallActivityAsync<bool>(nameof(PublishAnswerActivity), (question.Id, answer, robot));
            if (!pubResult) return new WorkflowResult("回答发布失败", false);
            
            return new WorkflowResult($"问题[{question.Id}]以由Robot[{robot.Name}]回答成功", true);
        }
        catch (Exception e)
        {
            return new WorkflowResult($"问题[{question.Id}]回答失败，错误信息：{e.Message}", false);
        }
    }
}