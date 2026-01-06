using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Zhihu.FeedService.Core.Entities;

namespace Zhihu.FeedService.Infrastructure.Contexts;

public class FeedDbContext(DbContextOptions<FeedDbContext> options) : DbContext(options)
{
    public DbSet<Outbox> Outbox => Set<Outbox>();

    public DbSet<Inbox> Inbox => Set<Inbox>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}
