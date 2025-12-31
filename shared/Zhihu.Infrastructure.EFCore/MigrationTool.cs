using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Zhihu.Infrastructure.EFCore;

public static class MigrationTool
{
    public static async Task RunMigrationAsync(DbContext dbContext, CancellationToken cancellationToken)
    {
        var strategy = dbContext.Database.CreateExecutionStrategy();
        await strategy.ExecuteAsync(async () =>
        {
            await dbContext.Database.MigrateAsync(cancellationToken);
        });
    }

    public static async Task SeedDataAsync<T>(DbContext dbContext, T[] entities, CancellationToken cancellationToken)
    {
        var strategy = dbContext.Database.CreateExecutionStrategy();
        await strategy.ExecuteAsync(async () =>
        {
            await using var transaction = await dbContext.Database.BeginTransactionAsync(cancellationToken);
            await dbContext.AddRangeAsync(entities);
            await dbContext.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);
        });
    }
}