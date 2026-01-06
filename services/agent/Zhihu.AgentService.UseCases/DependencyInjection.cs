using System.Reflection;
using Dapr.Workflow;
using Microsoft.Extensions.DependencyInjection;
using Zhihu.UseCases.Common;
using Zhihu.AgentService.UseCases.Activities.QuestionReply;
using Zhihu.AgentService.UseCases.Workflows;

namespace Zhihu.AgentService.UseCases;

public static class DependencyInjection
{
    public static IServiceCollection AddUseCaseService(this IServiceCollection services)
    {
        services.AddUseCaseCommon(Assembly.GetExecutingAssembly());
        
        services.AddDaprWorkflow(options =>
        {
            options.RegisterWorkflow<QuestionReplyingWorkflow>();
            options.RegisterActivity<RobotSelectActivity>();
            options.RegisterActivity<AnswerGenerateActivity>();
            options.RegisterActivity<PublishAnswerActivity>();
        });
        
        return services;
    }
}