using System.Reflection;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Zhihu.UserService.Core.Entities;

namespace Zhihu.UserService.Infrastructure.Contexts;

public class UserDbContext(DbContextOptions<UserDbContext> options)
    : IdentityUserContext<IdentityUser, int>(options)
{
    public DbSet<AppUser> AppUsers => Set<AppUser>();

    public DbSet<FollowUser> FollowUsers => Set<FollowUser>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}

//  Add-Migration InitialCreate -OutputDir Data\Migrations