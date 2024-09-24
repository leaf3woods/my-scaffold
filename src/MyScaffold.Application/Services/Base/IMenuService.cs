
using MyScaffold.Application.Dtos;

namespace MyScaffold.Application.Services.Base
{
    public interface IMenuService : IBaseService
    {
        Task<IEnumerable<MenuReadDto>> GetAllMenusAsync();

        Task<IEnumerable<MenuReadDto>> GetRootMenusAsTreeAsync();

        Task<int> SetMenuRoute();
    }
}
