using Microsoft.EntityFrameworkCore;
using MyScaffold.Application.Dtos;
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

        public Task<IEnumerable<MenuReadDto>> GetAllMenusAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<MenuReadDto>> GetRootMenusAsTreeAsync()
        {
            var entities = await _dbContext.Menus.ToArrayAsync();
            var menus = Mapper.Map<IEnumerable<MenuReadDto>>(entities);
            var root = AsTree(menus.Single(m => m.ParentId == null));
            return root.Children!;

            MenuReadDto AsTree(MenuReadDto parent)
            {
                parent.Children = menus
                    .Where(m => m.ParentId == parent.Id)
                    .Select(m => AsTree(m));
                return parent;
            }      
        }

        public Task<int> SetMenuRoute()
        {
            throw new NotImplementedException();
        }
    }
}
