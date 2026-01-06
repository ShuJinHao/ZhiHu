using System.Text;
using Dapr.Workflow;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Connectors.OpenAI;
using Zhihu.AgentService.UseCases.Queries;
using Zhihu.AgentService.UseCases;

namespace Zhihu.AgentService.UseCases.Activities.QuestionReply;

public class RobotSelectActivity(ISender sender, Kernel kernel, ILogger<RobotSelectActivity> logger) : WorkflowActivity<string, RobotDto?>
{
    public override async Task<RobotDto?> RunAsync(WorkflowActivityContext context, string input)
    {
        var robotsResult = await sender.Send(new GetRobotsForBaseInfoQuery(true));

        if (!robotsResult.IsSuccess)
        {
            logger.LogError("获取机器人列表失败");
            return null;
        }
        
        var robots = robotsResult.Value!;
        const string prompt = """
                              你的任务是根据给定的问题内容，识别该问题所属的知识领域。知识领域的范围限定在提供的知识领域列表中。
                              
                              首先，请仔细阅读以下知识领域列表：
                              <知识领域列表>
                              {{ $knowledge }}
                              </知识领域列表>
                              
                              现在，请仔细阅读以下问题：
                              <问题>
                              {{ $question }}
                              </问题>
                              
                              你只能输出知识领域列表中的纯数字ID。
                              如果列表中不存在该问题的知识领域，你只能输出数字 -1。
                              """;

        var knowledge = new StringBuilder();
        foreach (var robot in robots)
        {
            knowledge.AppendLine($"{robot.Id}.{robot.Knowledge}");
        }
        
        var arguments = new KernelArguments(new OpenAIPromptExecutionSettings { Temperature = 0.1 })
        {
            ["question"] = input,
            ["knowledge"] = knowledge
        };

        var reply = await kernel.InvokePromptAsync(prompt, arguments);
        var result = int.TryParse(reply.ToString(), out var id);
        if (!result || id == -1)
        {
            logger.LogError("无法识别问题所属知识领域");
            return null;
        }
        
        var robotResult = await sender.Send(new GetRobotById(id));
        if (!robotResult.IsSuccess)
        {
            logger.LogError("机器人[{id}]查询失败", id);
            return null;
        }
        
        return robotResult.Value;
    }
}