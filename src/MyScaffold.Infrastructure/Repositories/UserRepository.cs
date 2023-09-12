using MyScaffold.Domain.Entities;
using MyScaffold.Domain.Repositories;
using MyScaffold.Infrastructure.DbContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace MyScaffold.Infrastructure.Repositories
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(
            IDbContextFactory<PgDbContext> dbContextFactory,
            IConfiguration configuration
        ) : base(dbContextFactory, configuration) { }

        public async Task<int> DeleteAsync(Guid key)
        {
            var user = await DbContext.Set<User>()
                .FindAsync(key);
            if (user is null)
            {
                throw new Exception("user id is not exist!");
            }
            DbContext.Set<User>().Remove(user);
            return await DbContext.SaveChangesAsync();
        }

        public async Task<User?> FindAsync(Guid key, bool tracking = true)
        {
            var set = DbContext.Set<User>();
            var query = tracking ? set : set.AsNoTracking();
            var result = await query.Include(x => x.Role)
                .SingleOrDefaultAsync(u => u.Id == key);
            return result;
        }

        public async Task<User?> FindAsync(string username, bool tracking = true)
        {
            var set = DbContext.Set<User>();
            var query = tracking ? set : set.AsNoTracking();
            var result = await query.Include(x => x.Role)
                .SingleOrDefaultAsync(u => u.Username == username);
            return result;
        }

        public async Task<int> UpdateAsync(User modifiedUser)
        {
            DbContext.Set<User>().Update(modifiedUser);
            return await DbContext.SaveChangesAsync();
        }
    }
}