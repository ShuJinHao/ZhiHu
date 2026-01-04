using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Zhihu.QuestionService.Core.Data;
using Zhihu.QuestionService.Core.Entities;

namespace Zhihu.QuestionService.Infrastructure.Configuration;

public class QuestionConfiguration : IEntityTypeConfiguration<Question>
{
    public void Configure(EntityTypeBuilder<Question> builder)
    {
        builder.Property(p => p.Title)
            .HasMaxLength(DataSchemaConstants.DefaultQuestionTitleLength)
            .IsRequired();

        builder.Property(p => p.Description)
            .HasColumnType("text")
            .HasMaxLength(DataSchemaConstants.DefaultDescriptionTitleLength);

        builder.Property(p => p.Summary)
            .HasMaxLength(DataSchemaConstants.DefaultQuestionSummaryLength);
    }
}
