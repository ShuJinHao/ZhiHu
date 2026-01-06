using System.ComponentModel;
using Dapr;
using Dapr.Workflow;
using Microsoft.AspNetCore.Mvc;
using Zhihu.AgentService.UseCases.Workflows;
using Zhihu.SharedModels;
using Zhihu.SharedModels.Question;

namespace Zhihu.AgentService.HttpApi.Controllers;

[Tags("管理智能体工作流")]
[Route("Workflow")]
[ApiController]
public class WorkflowController(DaprWorkflowClient workflowClient, ILogger<WorkflowController> logger) : ControllerBase
{
    [ApiExplorerSettings(IgnoreApi = true)]
    [HttpPost("QuestionCreatedEvent")]
    [Topic(DaprContacts.PubSubComponent, nameof(QuestionCreatedEvent))]
    public async Task<IActionResult> QuestionCreated(QuestionCreatedEvent question)
    {
        var instanceId = $"qr-{question.Id}";
        logger.LogInformation("启动工作流: {instanceId}", instanceId);
        await workflowClient.ScheduleNewWorkflowAsync(nameof(QuestionReplyingWorkflow), instanceId, question);
        return Ok();
    }
    
    [EndpointSummary("查询智能体工作流状态")]
    [HttpGet("{instanceId}")]
    public async Task<IActionResult> GetState([Description("工作流ID")]string instanceId)
    {
        // 获取工作流状态
        var state = await workflowClient.GetWorkflowStateAsync(instanceId);
        
        if (state is not { Exists: true }) return NotFound();
        return state.RuntimeStatus switch
        {
            WorkflowRuntimeStatus.Completed => Ok(new { result = state.ReadOutputAs<WorkflowResult>() }),
            WorkflowRuntimeStatus.Failed => Ok(state),
            _ => Ok(state)
        };
    }
}