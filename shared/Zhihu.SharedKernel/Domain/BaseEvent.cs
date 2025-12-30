using MediatR;

namespace Zhihu.SharedKernel.Domain;

public abstract class BaseEvent : INotification
{
    public DateTimeOffset DateOccurred { get; set; } = DateTimeOffset.Now;
}