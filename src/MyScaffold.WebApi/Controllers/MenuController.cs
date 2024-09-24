using Microsoft.AspNetCore.Mvc;
using MyScaffold.Application.Services.Base;

namespace MyScaffold.WebApi.Controllers
{
    /// <summary>
    ///     菜单
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class MenuController : ControllerBase
    {
        public MenuController(IMenuService menuService)
        {
            _menuService = menuService;
        }

        private readonly IMenuService _menuService;
    }
}
