using MyScaffold.Application.Services.Base;
using MyScaffold.Infrastructure.DbContexts;

namespace MyScaffold.Application.Services
{
    public class MenuService : BaseService, IMenuService
    {
        public MenuService(
            ApiDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        private readonly ApiDbContext _dbContext;
    }
}
