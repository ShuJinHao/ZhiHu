using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Zhihu.QuestionService.Core.Entities;

namespace Zhihu.QuestionService.Infrastructure.Contexts;

public class QuestionDbContext(DbContextOptions<QuestionDbContext> options)
    : DbContext(options)
{
    public DbSet<Question> Questions => Set<Question>();

    public DbSet<Answer> Answers => Set<Answer>();

    public DbSet<AnswerLike> AnswerLikes => Set<AnswerLike>();

    public DbSet<FollowUser> FollowUsers => Set<FollowUser>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}
