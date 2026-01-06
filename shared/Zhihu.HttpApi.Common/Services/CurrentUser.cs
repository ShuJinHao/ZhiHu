using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Zhihu.Core.Common.Interfaces;
using Zhihu.SharedModels.Enums;

namespace Zhihu.HttpApi.Common.Services;

public class CurrentUser(IHttpContextAccessor httpContextAccessor) : IUser
{
    public readonly ClaimsPrincipal? User = httpContextAccessor.HttpContext?.User;

    public string? Username => User?.FindFirstValue(ClaimTypes.Name);

    public int? Id
    {
        get
        {
            var id = User?.FindFirstValue(ClaimTypes.NameIdentifier);

            if (id is null) return null;

            return Convert.ToInt32(id);
        }
    }

    public UserType UserType
    {
        get
        {
            var value = User?.FindFirstValue(nameof(UserType));

            return value is null ? UserType.AppUser : Enum.Parse<UserType>(value);
        }
    }
}