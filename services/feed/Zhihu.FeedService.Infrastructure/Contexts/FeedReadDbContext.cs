using Microsoft.EntityFrameworkCore;

namespace Zhihu.FeedService.Infrastructure.Contexts;

public class FeedReadDbContext(DbContextOptions<FeedDbContext> options) : FeedDbContext(options);