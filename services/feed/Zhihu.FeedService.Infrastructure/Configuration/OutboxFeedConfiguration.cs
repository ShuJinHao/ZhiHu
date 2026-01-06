using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Zhihu.FeedService.Core.Entities;

namespace Zhihu.FeedService.Infrastructure.Configuration;

public class OutboxFeedConfiguration : IEntityTypeConfiguration<Outbox>
{
    public void Configure(EntityTypeBuilder<Outbox> builder)
    {
        builder.Property(p => p.FeedType)
            .HasColumnType("tinyint")
            .IsRequired();
    }
}
