using MyScaffold.Domain.Entities;

namespace MyScaffold.Domain.Repositories
{
    public interface IUserRepository : IRepository<User>
    {
        public Task<int> DeleteAsync(Guid key);

        public Task<User?> FindAsync(Guid key, bool tracking = true);

        public Task<User?> FindAsync(string username, bool tracking = true);

        public Task<int> UpdateAsync(User modifiedUser);
    }
}