using Dapr.Workflow;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Connectors.OpenAI;
using Zhihu.AgentService.UseCases;

namespace Zhihu.AgentService.UseCases.Activities.QuestionReply;

public class AnswerGenerateActivity(Kernel kernel) : WorkflowActivity<(string question, RobotDto robot), string?>
{
    public override async Task<string?> RunAsync(WorkflowActivityContext context, (string question, RobotDto robot) input)
    {
        const string prompt = """
                              你将扮演一个在[{{ $knowledge }}]知识领域的专家，负责回答该知识领域的问题。
                              你所处的知识领域是：{{ $knowledge }}
                              你的回答风格为：{{ $character }}
                              以下是额外信息：
                              {{ $extra }}

                              你需要回答的问题是：
                              <question>
                              {{ $question }}
                              </question>
                              在回答问题时，请遵循以下指南：
                              1. 回答要丰富、全面，确保涵盖问题的各个方面。
                              2. 回答内容要紧密围绕你所处的知识领域。
                              3. 回答风格要符合设定的风格。
                              4. 可结合额外信息来完善回答，但不要偏离问题核心。
                              """;
        
        var arguments = new KernelArguments(new OpenAIPromptExecutionSettings { Temperature = input.robot.Temperature })
        {
            ["question"] = input.question,
            ["knowledge"] = input.robot.Knowledge,
            ["character"] = input.robot.Character ?? string.Empty,
            ["extra"] = input.robot.ExtraPrompt ?? string.Empty
        };
        
        var result = await kernel.InvokePromptAsync(prompt, arguments);
        return result.ToString();
    }
}