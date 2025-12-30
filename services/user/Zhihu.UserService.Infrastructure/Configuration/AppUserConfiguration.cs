using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Zhihu.UserService.Core.Data;
using Zhihu.UserService.Core.Entities;

namespace Zhihu.UserService.Infrastructure.Configuration;

public class AppUserConfiguration : IEntityTypeConfiguration<AppUser>
{
    public void Configure(EntityTypeBuilder<AppUser> builder)
    {
        builder.Property(p => p.Id)
            .ValueGeneratedNever();

        builder.Property(p => p.Nickname)
            .HasMaxLength(DataSchemaConstants.DefaultAppUserNickNameLength);

        builder.Property(p => p.Bio)
            .HasMaxLength(DataSchemaConstants.DefaultAppUserBioLength);
    }
}