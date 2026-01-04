using Dapr.Client;
using Zhihu.SharedModels;

namespace Zhihu.Infrastructure.MessageBus;

public class MessageBusService(DaprClient daprClient) : IMessageBusService
{
    public async Task PublishAsync<TMessage>(TMessage message) where TMessage : class
    {
        var topicName = typeof(TMessage).Name;
        await daprClient.PublishEventAsync(DaprContacts.PubSubComponent, topicName, message);
    }
}