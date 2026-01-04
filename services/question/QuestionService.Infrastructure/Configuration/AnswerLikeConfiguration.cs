using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Zhihu.QuestionService.Core.Entities;

namespace Zhihu.QuestionService.Infrastructure.Configuration;

public class AnswerLikeConfiguration : IEntityTypeConfiguration<AnswerLike>
{
    public void Configure(EntityTypeBuilder<AnswerLike> builder)
    {
        // 设置组合唯一约束
        builder
            .HasIndex(a => new { a.AnswerId, a.UserId })
            .IsUnique();

        // 设置回答与点赞记录列表之间的一对多关系
        builder.HasOne(a => a.Answer)
            .WithMany(a => a.AnswerLikes)
            .HasForeignKey(a => a.AnswerId)
            .IsRequired()
            .OnDelete(DeleteBehavior.ClientCascade);
    }
}