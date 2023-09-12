using MyScaffold.Domain.Entities;
using MyScaffold.Domain.Repositories;
using MyScaffold.Infrastructure.DbContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace MyScaffold.Infrastructure.Repositories
{
    public class RoleRepository : Repository<Role>, IRoleRepository
    {
        public RoleRepository(
                IDbContextFactory<PgDbContext> dbContextFactory,
                IConfiguration configuration
            ) : base(dbContextFactory, configuration) { }

        public async Task<int> DeleteAsync(Guid key)
        {
            var role = await DbContext.Set<Role>().FindAsync(key);
            if (role is null)
            {
                throw new Exception("role not exist");
            }
            DbContext.Set<Role>().Remove(role);
            return await DbContext.SaveChangesAsync();
        }

        public async Task<Role?> FindAsync(Guid key)
        {
            return await DbContext.Set<Role>().FindAsync(key);
        }

        public async Task<int> UpdateAsync(Role modifiedRole)
        {
            DbContext.Set<Role>().Update(modifiedRole);
            return await DbContext.SaveChangesAsync();
        }
    }
}