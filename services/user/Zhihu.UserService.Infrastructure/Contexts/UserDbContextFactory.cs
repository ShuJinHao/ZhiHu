using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Zhihu.UserService.Infrastructure.Contexts;

// 这个类只有在你运行 dotnet ef 命令时才会被调用
// 实际项目启动时不会运行它
public class UserDbContextFactory : IDesignTimeDbContextFactory<UserDbContext>
{
    public UserDbContext CreateDbContext(string[] args)
    {
        // 1. 既然只是为了生成代码，不需要真的去连数据库探测版本
        // 直接假装我们在用 MySQL 8.0 即可
        var optionsBuilder = new DbContextOptionsBuilder<UserDbContext>();
        var connectionString = "Server=localhost;Database=zhihu_user_migration_temp;User=root;Password=placeholder;";

        // 这里直接写死版本，为了让 EF 工具能工作
        optionsBuilder.UseMySql(connectionString, new MySqlServerVersion(new Version(8, 0, 0)));

        return new UserDbContext(optionsBuilder.Options);
    }
}