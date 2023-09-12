using MyScaffold.Application.Auth;
using MyScaffold.Application.Dtos;
using MyScaffold.Application.Services.Base;
using MyScaffold.WebApi.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MyScaffold.WebApi.Controllers
{
    /// <summary>
    ///     角色资源
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = ManagedResource.Role)]
    public class RoleController : ControllerBase
    {
        /// <summary>
        ///     注入
        /// </summary>
        /// <param name="roleService"></param>
        public RoleController(
            IRoleService roleService
            )
        {
            _roleService = roleService;
        }

        private readonly IRoleService _roleService;

        /// <summary>
        ///     获取角色信息
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("{roleId:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Authorize(Policy = $"{ManagedResource.Role}.{ManagedAction.Read}.{ManagedItem.Id}")]
        public async Task<ResponseWrapper<RoleReadDto?>> GetRole(Guid roleId) =>
            (await _roleService.GetRoleAsync(roleId)).Wrap();

        /// <summary>
        ///     获取所有角色
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("all")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Authorize(Policy = $"{ManagedResource.Role}.{ManagedAction.Read}.{ManagedItem.All}")]
        public async Task<ResponseWrapper<IEnumerable<RoleReadDto>>> GetRoles() =>
            (await _roleService.GetRolesAsync()).Wrap();

        /// <summary>
        ///     创建新角色
        /// </summary>
        /// <param name="roleDto"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Authorize(Policy = $"{ManagedResource.Role}.{ManagedAction.Create}.{ManagedItem.Dto}")]
        public async Task<ResponseWrapper<RoleReadDto?>> CreateRole(RoleCreateDto roleDto) =>
            (await _roleService.CreateRoleAsync(roleDto)).Wrap();

        /// <summary>
        ///     编辑角色权限范围
        /// </summary>
        /// <param name="roleId"></param>
        /// <param name="scopes"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("{roleId:guid}/scopes")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Authorize(Policy = $"{ManagedResource.Role}.{ManagedAction.Update}.{"Scopes"}")]
        public async Task<ResponseWrapper<int>> ModifyRoleScopeAsync(Guid roleId, List<string> scopes) =>
            (await _roleService.ModifyRoleScopeAsync(roleId, scopes)).Wrap();

        /// <summary>
        ///     获取支持的权限范围
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("scopes")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Authorize(Policy = $"{ManagedResource.Role}.{ManagedAction.Read}.{"Scopes"}")]
        public ResponseWrapper<IEnumerable<RoleScopeReadDto>> GetSupportedScopes() =>
            _roleService.GetScopes().Wrap();
    }
}