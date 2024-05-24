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
    [ScopeDefinition("manage all role resources", ManagedResource.Role)]
    public class RoleService : BaseService, IRoleService
    {
        public RoleService(
            IRoleRepository roleRepository
            )
        {
            _roleRepository = roleRepository;
        }

        private readonly IRoleRepository _roleRepository;

        [ScopeDefinition("create a role", $"{ManagedResource.Role}.{ManagedAction.Create}.One")]
        public async Task<RoleReadDto?> CreateRoleAsync(RoleCreateDto roleDto)
        {
            if (!RequireScopeUtil.Scopes.Any(s => !roleDto.ScopeNames.Contains(s.Name)))
            {
                throw new NotAcceptableException("unsupported scope find");
            }
            var role = Mapper.Map<Role>(roleDto);
            var index = await _roleRepository.CreateAsync(role);
            var dto = Mapper.Map<RoleReadDto>(role);
            return index == 0 ? null : dto;
        }

        [ScopeDefinition("get role info by id", $"{ManagedResource.Role}.{ManagedAction.Read}.One")]
        public async Task<RoleReadDto?> GetRoleAsync(Guid id)
        {
            var role = await _roleRepository
                .GetQueryWhere(r => r.Id == id)
                .Include(r => r.Scopes)
                .FirstOrDefaultAsync();
            return Mapper.Map<RoleReadDto>(role);
        }

        [ScopeDefinition("get all roles", $"{ManagedResource.Role}.{ManagedAction.Read}.All")]
        public async Task<IEnumerable<RoleReadDto>> GetRolesAsync()
        {
            var roles = await _roleRepository
                .GetQueryWhere()
                .Include(r => r.Scopes)
                .ToArrayAsync();
            return Mapper.Map<IEnumerable<RoleReadDto>>(roles);
        }

        [ScopeDefinition("change role manage scope", $"{ManagedResource.Role}.{ManagedAction.Update}.Scope")]
        public async Task<int> ModifyRoleScopeAsync(Guid roleId, List<string> scopeNames)
        {
            if (!RequireScopeUtil.Scopes.Any(s => !scopeNames.Contains(s.Name)))
            {
                throw new NotAcceptableException("unsupported scope find");
            }
            var role = (await _roleRepository.FindAsync(roleId)) ??
                throw new NotFoundException("role is not exist");
            var result = await _roleRepository.UpdateAsync(role);
            return result;
        }

        [ScopeDefinition("get all supported scopes", $"{ManagedResource.Role}.{ManagedAction.Read}.Scope")]
        public IEnumerable<RoleScopeReadDto> GetScopes() =>
            Mapper.Map<IEnumerable<RoleScopeReadDto>>(RequireScopeUtil.Scopes);
    }
}