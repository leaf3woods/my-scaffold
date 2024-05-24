using MyScaffold.Application.Auth;
using MyScaffold.Application.Dtos;
using MyScaffold.Application.Services.Base;
using MyScaffold.WebApi.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MyScaffold.WebApi.Controllers
{
    /// <summary>
    ///     用户资源
    /// </summary>
    [Route("api/[controller]")]
    [Authorize(Policy = ManagedResource.User)]
    [ApiController]
    public class UserController : ControllerBase
    {
        /// <summary>
        ///     注入服务
        /// </summary>
        /// <param name="userService">用户服务</param>
        public UserController(
            IUserService userService
            )
        {
            _userService = userService;
        }

        private readonly IUserService _userService;

        /// <summary>
        ///     获取指定Id的用户
        /// </summary>
        /// <param name="userId">GUID</param>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        [Route("{userId:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Authorize(Policy = $"{ManagedResource.User}.{ManagedAction.Read}.{ManagedItem.Id}")]
        public async Task<ResponseWrapper<UserReadDto?>> GetUser(Guid userId) =>
            (await _userService.GetUserAsync(userId)).Wrap();

        /// <summary>
        ///     模糊匹配用户名和昵称
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("where")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Authorize(Policy = $"{ManagedResource.User}.{ManagedAction.Read}.{ManagedItem.All}")]
        public async Task<ResponseWrapper<IEnumerable<UserReadDto>>> GetUsers(string? name = null) =>
            (await _userService.GetUsersWhereAsync(name)).Wrap();

        /// <summary>
        ///     删除用户
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("{userId:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Authorize(Policy = $"{ManagedResource.User}.{ManagedAction.Delete}.{ManagedItem.Id}")]
        public async Task<ResponseWrapper<int>> Delete(Guid userId) =>
            (await _userService.DeleteAsync(userId)).Wrap();

        /// <summary>
        ///     切换权限
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="roleId"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("{userId:guid}/role/{roleId:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ResponseWrapper<UserReadDto?>> ModifyRole(Guid userId, Guid roleId) =>
            (await _userService.ChangeRoleAsync(userId, roleId)).Wrap();

        /// <summary>
        ///     更换自己的密码
        /// </summary>
        /// <param name="passwordDto"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("pwd")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ResponseWrapper<int>> ChangePassword(ChangePasswordDto passwordDto) =>
            (await _userService.ChangePasswordAsync(passwordDto)).Wrap();

        /// <summary>
        ///     重置某个用户的密码
        /// </summary>
        /// <returns></returns>
        [HttpPut]
        [Route("{userId}/pwd")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Authorize(Policy = $"{ManagedResource.User}.{ManagedAction.Delete}.AllPWD")]
        public async Task<ResponseWrapper<int>> ResetPassword(Guid userId) =>
            (await _userService.ResetPasswordAsync(userId)).Wrap();
    }
}