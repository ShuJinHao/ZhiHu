using System.Reflection;
using MediatR;
using Zhihu.Core.Common.Interfaces;
using Zhihu.UseCases.Common.Exceptions;
using Zhihu.UseCases.Common.Attributes;

namespace Zhihu.UseCases.Common.Behaviors;

public class AuthorizationBehavior<TRequest, TResponse>(IUser user) : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        var authorizeAttributes = request.GetType().GetCustomAttributes<AuthorizeAttribute>();

        if (authorizeAttributes.Any())
            if (user.Id is null)
                throw new ForbiddenException();

        // 其它授权逻辑

        return await next();
    }
}