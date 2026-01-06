using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Zhihu.FeedService.Core.Entities;

namespace Zhihu.FeedService.Infrastructure.Configuration;

public class InboxFeedConfiguration : IEntityTypeConfiguration<Inbox>
{
    public void Configure(EntityTypeBuilder<Inbox> builder)
    {
        builder.Property(p => p.FeedType)
            .HasColumnType("tinyint")
            .IsRequired();
    }
}