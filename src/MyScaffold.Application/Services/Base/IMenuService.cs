﻿
using MyScaffold.Application.Dtos;

namespace MyScaffold.Application.Services.Base
{
    public interface IMenuService : IBaseService
    {
        Task<IEnumerable<MenuReadDto>> GetAllMenusAsync();

        Task<IEnumerable<MenuReadDto>> GetRootMenusAsTreeAsync();

        Task<int> DeleteMenuAsync(Guid id, bool force);

        Task<MenuReadDto> CreateMenuAsync(MenuCreateDto dto);

        Task<int> SetMenuRouteAsync();
    }
}
