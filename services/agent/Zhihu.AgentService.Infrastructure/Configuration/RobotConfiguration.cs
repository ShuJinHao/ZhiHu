using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Zhihu.AgentService.Core.Entities;

namespace Zhihu.AgentService.Infrastructure.Configuration;

public class RobotConfiguration : IEntityTypeConfiguration<Robot>
{
    public void Configure(EntityTypeBuilder<Robot> builder)
    {
        builder.Property(p => p.Knowledge)
            .IsRequired();

        builder.Property(p => p.Character)
            .HasColumnType("text");

        builder.Property(p => p.ExtraPrompt)
            .HasColumnType("text");
    }
}