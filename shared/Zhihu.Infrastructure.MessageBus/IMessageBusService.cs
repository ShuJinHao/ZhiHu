namespace Zhihu.Infrastructure.MessageBus;

public interface IMessageBusService
{
    Task PublishAsync<TMessage>(TMessage message) where TMessage : class;
}
