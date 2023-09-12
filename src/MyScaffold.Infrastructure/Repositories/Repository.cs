using MyScaffold.Core;
using MyScaffold.Domain.Entities.Base;
using MyScaffold.Domain.Repositories;
using MyScaffold.Infrastructure.DbContexts;
using MyScaffold.Infrastructure.Utilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Linq.Expressions;

namespace MyScaffold.Infrastructure.Repositories
{
    public abstract class Repository<TEntity> : IRepository<TEntity>
        where TEntity : class, IAggregateRoot, new()
    {
        public Repository(
                IDbContextFactory<PgDbContext> dbContextFactory,
                IConfiguration configuration
            )
        {
            _lazyContext = new Lazy<PgDbContext>(dbContextFactory.CreateDbContext());
            _configuration = configuration;
        }

        private readonly IConfiguration _configuration;
        private Lazy<PgDbContext> _lazyContext { get; init; }
        public PgDbContext DbContext { get => _lazyContext.Value; }

        public async Task<int> CreateAsync(TEntity entity)
        {
            await DbContext.Set<TEntity>().AddAsync(entity);
            var count = await DbContext.SaveChangesAsync();
            return count;
        }

        public async Task<int> CreateRangeAsync(IEnumerable<TEntity> entities)
        {
            if (entities.Count() != 0)
            {
                await DbContext.Set<TEntity>().AddRangeAsync(entities);
            }
            var count = await DbContext.SaveChangesAsync();
            return count;
        }

        public IQueryable<TEntity> GetQueryWhere(Expression<Func<TEntity, bool>>? expression = null, bool track = true)
        {
            var set = track ?
                DbContext.Set<TEntity>() :
                DbContext.Set<TEntity>().AsNoTracking();
            var query = expression == null ?
                set :
                set.Where(expression);
            return query;
        }

        public async Task<PaginatedList<TEntity>> GetPaginatedAsync(int pageIndex, int pageSize)
        {
            var results = await DbContext.Set<TEntity>()
                .AsNoTracking()
                .ToPaginatedListAsync(pageIndex, pageSize);
            return results;
        }

        public async Task<int> Update(TEntity entity)
        {
            DbContext.Set<TEntity>().Update(entity);
            return await DbContext.SaveChangesAsync();
        }
    }
}