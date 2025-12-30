using MediatR;

namespace Zhihu.SharedKernel.Messaging;

public interface IQuery<out TResponse> : IRequest<TResponse>
{
}