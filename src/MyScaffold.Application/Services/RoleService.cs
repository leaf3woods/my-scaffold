using MyScaffold.Application.Auth;
using MyScaffold.Application.Dtos;
using MyScaffold.Application.Services.Base;
using MyScaffold.Core.Exceptions;
using MyScaffold.Domain.Entities;
using MyScaffold.Domain.Repositories;
using MyScaffold.Domain.Utilities;
using Microsoft.EntityFrameworkCore;

namespace MyScaffold.Application.Services
{
    [Scope("manage all role resources", ManagedResource.Role)]
    public class RoleService : BaseService, IRoleService
    {
        public RoleService(
            IRoleRepository roleRepository
            )
        {
            _roleRepository = roleRepository;
        }

        private readonly IRoleRepository _roleRepository;

        [Scope("create a role", ManagedResource.Role, ManagedAction.Create, ManagedItem.Dto)]
        public async Task<RoleReadDto?> CreateRoleAsync(RoleCreateDto roleDto)
        {
            if (!RequireScopeUtil.TryFillAll(roleDto.ScopeNames, out var fullScopes))
            {
                throw new NotAcceptableException("unsupported scope find");
            }
            var role = Mapper.Map<Role>(roleDto);
            role.Scopes = fullScopes;
            var index = await _roleRepository.CreateAsync(role);
            var dto = Mapper.Map<RoleReadDto>(role);
            return index == 0 ? null : dto;
        }

        [Scope("get role info by id", ManagedResource.Role, ManagedAction.Read, ManagedItem.Id)]
        public async Task<RoleReadDto?> GetRoleAsync(Guid id)
        {
            var role = await _roleRepository
                .GetQueryWhere(r => r.Id == id)
                .Include(r => r.Scopes)
                .FirstOrDefaultAsync();
            return Mapper.Map<RoleReadDto>(role);
        }

        [Scope("get all roles", ManagedResource.Role, ManagedAction.Read, ManagedItem.All)]
        public async Task<IEnumerable<RoleReadDto>> GetRolesAsync()
        {
            var roles = await _roleRepository
                .GetQueryWhere()
                .Include(r => r.Scopes)
                .ToArrayAsync();
            return Mapper.Map<IEnumerable<RoleReadDto>>(roles);
        }

        [Scope("change role manage scope", ManagedResource.Role, ManagedAction.Update, "Scopes")]
        public async Task<int> ModifyRoleScopeAsync(Guid roleId, List<string> scopes)
        {
            if (!RequireScopeUtil.TryFillAll(scopes, out var fullScopes))
            {
                throw new NotAcceptableException("unsupported scope find");
            }
            var role = (await _roleRepository.FindAsync(roleId)) ??
                throw new NotFoundException("role is not exist");
            role.Scopes = fullScopes;
            var result = await _roleRepository.UpdateAsync(role);
            return result;
        }

        [Scope("get all supported scopes", ManagedResource.Role, ManagedAction.Read, "Scopes")]
        public IEnumerable<RoleScopeReadDto> GetScopes()
        {
            var result = RequireScopeUtil.Scopes.Select(Mapper.Map<RoleScopeReadDto>);
            return result;
        }
    }
}