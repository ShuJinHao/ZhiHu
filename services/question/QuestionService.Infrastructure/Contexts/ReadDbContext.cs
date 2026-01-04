using Microsoft.EntityFrameworkCore;

namespace Zhihu.QuestionService.Infrastructure.Contexts;

public class QuestionReadDbContext(DbContextOptions<QuestionDbContext> options) : QuestionDbContext(options)
{

}
