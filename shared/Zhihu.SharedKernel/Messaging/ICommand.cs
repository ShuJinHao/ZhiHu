using MediatR;

namespace Zhihu.SharedKernel.Messaging;

public interface ICommand<out TResponse> : IRequest<TResponse>
{
}