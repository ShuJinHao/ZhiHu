using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Zhihu.UseCases.Common.Exceptions;

namespace Zhihu.HttpApi.Common.Infrastructure;

public class UseCaseExceptionHandler : IExceptionHandler
{
    private readonly Dictionary<Type, Func<HttpContext, Exception, Task>> _exceptionHandlers = new()
    {
        { typeof(ValidationException), HandleValidationException },
        { typeof(ForbiddenException), HandleForbiddenExceptionAsync }
    };

    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception,
        CancellationToken cancellationToken)
    {
        var exceptionType = exception.GetType();

        if (!_exceptionHandlers.TryGetValue(exceptionType, out var handler)) return false;

        await handler.Invoke(httpContext, exception);
        return true;
    }

    private static async Task HandleValidationException(HttpContext httpContext, Exception exception)
    {
        var validationException = (ValidationException)exception;

        httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;

        await httpContext.Response.WriteAsJsonAsync(new ValidationProblemDetails(validationException.Errors)
        {
            Status = StatusCodes.Status400BadRequest,
            Type = "https://developer.mozilla.org/zh-CN/docs/Web/HTTP/Status/400"
        });
    }

    private static async Task HandleForbiddenExceptionAsync(HttpContext httpContext, Exception exception)
    {
        httpContext.Response.StatusCode = StatusCodes.Status403Forbidden;

        await httpContext.Response.WriteAsJsonAsync(new ProblemDetails
        {
            Status = StatusCodes.Status403Forbidden,
            Title = "Forbidden",
            Type = "https://developer.mozilla.org/zh-CN/docs/Web/HTTP/Status/403"
        });
    }
}