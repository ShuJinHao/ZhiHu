using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Zhihu.QuestionService.Core.Data;
using Zhihu.QuestionService.Core.Entities;

namespace Zhihu.QuestionService.Infrastructure.Configuration;

public class AnswerConfiguration : IEntityTypeConfiguration<Answer>
{
    public void Configure(EntityTypeBuilder<Answer> builder)
    {
        builder.Property(p => p.Content)
            .HasColumnType("text")
            .IsRequired();

        // 设置问题与回答列表之间的一对多关系
        builder.HasOne(a => a.Question)
            .WithMany(q => q.Answers)
            .HasForeignKey(a => a.QuestionId)
            .IsRequired()
            .OnDelete(DeleteBehavior.ClientCascade);

        builder.Property(p => p.Summary)
            .HasMaxLength(DataSchemaConstants.DefaulAnswerSummaryLength);
    }
}