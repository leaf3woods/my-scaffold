using MyScaffold.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace MyScaffold.Infrastructure.DbContexts
{
    public class InitialDatabase
    {
        public InitialDatabase(IDbContextFactory<PgDbContext> dbContextFactory)
        {
            _dbContextFactory = dbContextFactory;
        }

        private readonly IDbContextFactory<PgDbContext> _dbContextFactory;

        public async Task Initialize()
        {
            var context = await _dbContextFactory.CreateDbContextAsync();
            context.Database.EnsureCreated();
            await context.Database.MigrateAsync();
            foreach (var seed in Scope.Seeds)
            {
                var entity = await context.Scopes.FirstOrDefaultAsync(sc => sc.Id == seed.Id);
                if (entity is null)
                {
                    await context.Scopes.AddAsync(seed);
                }
            }
            await context.SaveChangesAsync();
        }
    }
}