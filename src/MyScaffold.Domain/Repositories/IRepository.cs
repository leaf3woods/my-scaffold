using MyScaffold.Core;
using MyScaffold.Domain.Entities.Base;
using System.Linq.Expressions;

namespace MyScaffold.Domain.Repositories
{
    public interface IRepository<TEntity>
        where TEntity : class, IAggregateRoot, new()
    {
        public Task<int> CreateAsync(TEntity dto);

        public Task<int> CreateRangeAsync(IEnumerable<TEntity> dtos);

        public IQueryable<TEntity> GetQueryWhere(Expression<Func<TEntity, bool>>? expression = null, bool track = true);

        public Task<PaginatedList<TEntity>> GetPaginatedAsync(int pageIndex, int pageSize);

        public Task<int> Update(TEntity entity);
    }
}