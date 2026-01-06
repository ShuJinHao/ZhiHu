using Microsoft.AspNetCore.Mvc;
using Zhihu.AgentService.UseCases.Commands;
using Zhihu.HttpApi.Common.Infrastructure;

namespace Zhihu.AgentService.HttpApi.Controllers;

[Tags("管理智能体机器人")]
public class RobotController : ApiControllerBase
{
    [EndpointSummary("创建机器人")]
    [HttpPost]
    public async Task<IActionResult> Create(CreateRobotCommand command)
    {
        return ReturnResult(await Sender.Send(command));
    }
}