using FluentValidation.Results;

namespace Zhihu.UseCases.Common.Exceptions;

public class ValidationException() : Exception("发生了一个或多个验证失败")
{
    public ValidationException(IEnumerable<ValidationFailure> failures)
        : this()
    {
        Errors = failures
            .GroupBy(failure => failure.PropertyName, failure => failure.ErrorMessage)
            .ToDictionary(grouping => grouping.Key, grouping => grouping.ToArray());
    }

    public IDictionary<string, string[]> Errors { get; } = new Dictionary<string, string[]>();
}