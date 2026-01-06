using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Zhihu.AgentService.Core.Entities;

namespace Zhihu.AgentService.Infrastructure.Contexts;

public class AgentDbContext(DbContextOptions<AgentDbContext> options) : DbContext(options)
{
    public DbSet<Robot> Robots => Set<Robot>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}