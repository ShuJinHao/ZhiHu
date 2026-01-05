using Dapr;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Zhihu.SharedModels;
using Zhihu.SharedModels.Question;

namespace Zhihu.HotService.HttpApi.Controllers;

[Route("Subscribe")]
[ApiController]
public class SubscribeController(IPublisher publisher) : ControllerBase
{
    [HttpPost("AnswerCreatedEvent")]
    [Topic(DaprContacts.PubSubComponent, nameof(AnswerCreatedEvent))]
    public async Task<IActionResult> AnswerCreated(AnswerCreatedEvent @event)
    {
        await publisher.Publish(@event);
        return Ok();
    }
    
    [HttpPost("AnswerLikedEvent")]
    [Topic(DaprContacts.PubSubComponent, nameof(AnswerLikedEvent))]
    public async Task<IActionResult> AnswerLiked(AnswerLikedEvent @event)
    {
        await publisher.Publish(@event);
        return Ok();
    }
    
    [HttpPost("FollowQuestionAddedEvent")]
    [Topic(DaprContacts.PubSubComponent, nameof(FollowQuestionAddedEvent))]
    public async Task<IActionResult> FollowQuestionAdded(FollowQuestionAddedEvent @event)
    {
        await publisher.Publish(@event);
        return Ok();
    }
    
    [HttpPost("QuestionViewedEvent")]
    [Topic(DaprContacts.PubSubComponent, nameof(QuestionViewedEvent))]
    public async Task<IActionResult> QuestionViewed(QuestionViewedEvent @event)
    {
        Console.WriteLine(@event.QuestionId);
        await publisher.Publish(@event);
        return Ok();
    }
}