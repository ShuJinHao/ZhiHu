using Dapr.Client;
using Microsoft.Extensions.Configuration;
using Zhihu.SharedKernel.Result;

namespace Zhihu.FeedService.Infrastructure.Repositories;

public class UserHttpRepository(DaprClient daprClient, IConfiguration configuration) : IUserHttpRepository
{
    public string AppId { get; set; } = configuration["AppId:UserService"] ?? throw new ArgumentNullException(nameof(AppId));
    public string BaseRouter { get; set; } = "api/appuser";

    public async Task<Result<List<int>>> GetFollowerIdsAsync(int userId)
    {
        var result = await daprClient.InvokeMethodAsync<List<int>>(
            HttpMethod.Get, 
            AppId, 
            $"{BaseRouter}/follow/user/{userId}");
        
        return result is null ? Result.Failure() : Result.Success(result!);
    }
}