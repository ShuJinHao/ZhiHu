using Microsoft.EntityFrameworkCore;

namespace Zhihu.UserService.Infrastructure.Contexts;

public class UserReadDbContext(DbContextOptions<UserDbContext> options) : UserDbContext(options)
{

}
