using MyScaffold.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace MyScaffold.Infrastructure.DbContexts
{
    public class InitialDatabase
    {
        public InitialDatabase(
            IDbContextFactory<PgDbContext> dbContextFactory,
            ILogger<InitialDatabase> logger
            )
        {
            _dbContextFactory = dbContextFactory;
            _logger = logger;
        }

        private readonly IDbContextFactory<PgDbContext> _dbContextFactory;
        private readonly ILogger<InitialDatabase> _logger;

        public async Task Initialize()
        {
            var context = await _dbContextFactory.CreateDbContextAsync();
            context.Database.EnsureCreated();
            await context.Database.MigrateAsync();
            var scopes = await context.Scopes
                .ToArrayAsync();
            var transaction = await context.Database.BeginTransactionAsync();
            try
            {
                if (scopes.Length != Scope.Seeds.Length || scopes.Any(ss => Scope.Seeds.Contains(ss)))
                {
                    context.Scopes.RemoveRange(scopes);
                    await context.SaveChangesAsync();
                    await context.AddRangeAsync(Scope.Seeds);
                }
                var count = await context.SaveChangesAsync();
                await transaction.CommitAsync();
                _logger.LogInformation($"scope table generate succeed, with {count} new scopes");
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
    }
}