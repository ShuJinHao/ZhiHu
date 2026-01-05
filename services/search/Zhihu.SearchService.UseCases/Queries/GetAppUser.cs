using FluentValidation;
using Zhihu.SearchService.Infrastructure;
using Zhihu.SharedKernel.Messaging;
using Zhihu.SharedKernel.Paging;
using Zhihu.SharedKernel.Result;
using Zhihu.SharedKernel.Search;
using Zhihu.SearchService.UseCases;

namespace Zhihu.SearchService.UseCases.Queries;

public record GetAppUserQuery(string Query, Pagination Pagination) : IQuery<Result<SearchResult<SearchAppUserDto>>>;

public class GetAppUserQueryValidator : AbstractValidator<GetAppUserQuery>
{
    public GetAppUserQueryValidator()
    {
        RuleFor(command => command.Query).NotEmpty();
    }
}

public class GetAppUserQueryHandler(ISearchService searchService)
    : IQueryHandler<GetAppUserQuery, Result<SearchResult<SearchAppUserDto>>>
{
    public async Task<Result<SearchResult<SearchAppUserDto>>> Handle(GetAppUserQuery request,
        CancellationToken cancellationToken)
    {
        const string indices = "appusers";
        const string field = "nickname";
        var result =
            await searchService.MatchQuery<SearchAppUserDto>(indices, field, request.Query, request.Pagination);

        return result.Count == 0 ? Result.NotFound() : Result.Success(result);
    }
}