namespace Zhihu.SharedKernel.Result;

public interface IResult
{
    IEnumerable<string>? Errors { get; }

    bool IsSuccess { get; }

    ResultStatus Status { get; }

    object? GetValue();
}