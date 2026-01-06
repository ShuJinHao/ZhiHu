using Microsoft.EntityFrameworkCore;

namespace Zhihu.AgentService.Infrastructure.Contexts;

public class AgentReadDbContext(DbContextOptions<AgentDbContext> options) : AgentDbContext(options);