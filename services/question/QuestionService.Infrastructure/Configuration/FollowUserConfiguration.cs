using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Zhihu.QuestionService.Core.Entities;

namespace Zhihu.QuestionService.Infrastructure.Configuration;

public class FollowUserConfiguration : IEntityTypeConfiguration<FollowUser>
{
    public void Configure(EntityTypeBuilder<FollowUser> builder)
    {
        // 设置组合唯一约束
        builder
            .HasIndex(fq => new { fq.UserId, fq.QuestionId })
            .IsUnique();

        // 设置用户与关注问题列表之间的一对多关系
        builder
            .HasOne(fq => fq.Question)
            .WithMany(u => u.FollowUsers)
            .HasForeignKey(fq => fq.UserId)
            .IsRequired()
            .OnDelete(DeleteBehavior.ClientCascade);
    }
}