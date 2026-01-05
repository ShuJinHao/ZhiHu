using FluentValidation;
using Zhihu.SearchService.Infrastructure;
using Zhihu.SharedKernel.Messaging;
using Zhihu.SharedKernel.Paging;
using Zhihu.SharedKernel.Result;
using Zhihu.SharedKernel.Search;
using Zhihu.SearchService.UseCases;

namespace Zhihu.SearchService.UseCases.Queries;

public record GetContentQuery(string Query, Pagination Pagination) : IQuery<Result<SearchResult<ContentDto>>>;

public class GetContentQueryValidator : AbstractValidator<GetContentQuery>
{
    public GetContentQueryValidator()
    {
        RuleFor(command => command.Query).NotEmpty();
    }
}

public class GetContentHandler(ISearchService searchService)
    : IQueryHandler<GetContentQuery, Result<SearchResult<ContentDto>>>
{
    public async Task<Result<SearchResult<ContentDto>>> Handle(GetContentQuery request,
        CancellationToken cancellationToken)
    {
        const string indices = "questions,answers";
        string[] fields = ["title", "content", "description"];

        var result =
            await searchService.ShouldMatchQuery<ContentDto>(indices, fields,
                request.Query, request.Pagination);

        return result.Count == 0 ? Result.NotFound() : Result.Success(result);
    }
}
