using MyScaffold.Application.Auth;
using MyScaffold.Application.Dtos;
using MyScaffold.Application.Services.Base;
using MyScaffold.Core.Exceptions;
using MyScaffold.Domain.Utilities;
using Microsoft.EntityFrameworkCore;
using MyScaffold.Infrastructure.DbContexts;
using MyScaffold.Domain.Entities.Login;

namespace MyScaffold.Application.Services
{
    [ScopeDefinition("manage all role resources", ManagedResource.Role)]
    public class RoleService : BaseService, IRoleService
    {
        public RoleService(
            ApiDbContext apiDbContext)
        {
            _apiDbContext = apiDbContext;
        }

        private readonly ApiDbContext _apiDbContext;

        [ScopeDefinition("create a role", $"{ManagedResource.Role}.{ManagedAction.Create}.New")]
        public async Task<RoleReadDto?> CreateRoleAsync(RoleCreateDto roleDto)
        {
            if (!RequireScopeUtil.Scopes.Any(s => !roleDto.ScopeNames.Contains(s.Name)))
            {
                throw new NotAcceptableException("unsupported scope find");
            }
            var entity = Mapper.Map<Role>(roleDto);
            await _apiDbContext.Roles.AddAsync(entity);
            var index = await _apiDbContext.SaveChangesAsync();
            return index == 0 ? null : Mapper.Map<RoleReadDto>(entity);
        }

        [ScopeDefinition("get role info by id", $"{ManagedResource.Role}.{ManagedAction.Read}.Id")]
        public async Task<RoleReadDto?> GetRoleAsync(Guid id)
        {
            var role = await _apiDbContext.Roles
                .Where(r => r.Id == id)
                .Include(r => r.Scopes)
                .AsNoTracking()
                .FirstOrDefaultAsync();
            return Mapper.Map<RoleReadDto>(role);
        }

        [ScopeDefinition("get all roles", $"{ManagedResource.Role}.{ManagedAction.Read}.All")]
        public async Task<IEnumerable<RoleReadDto>> GetRolesAsync()
        {
            var roles = await _apiDbContext.Roles
                .Include(r => r.Scopes)
                .AsNoTracking()
                .ToArrayAsync();
            return Mapper.Map<IEnumerable<RoleReadDto>>(roles);
        }

        [ScopeDefinition("change role manage scope", $"{ManagedResource.Role}.{ManagedAction.Update}.Scopes")]
        public async Task<int> ModifyRoleScopeAsync(Guid roleId, List<string> scopeNames)
        {
            if (!RequireScopeUtil.Scopes.Any(s => !scopeNames.Contains(s.Name)))
            {
                throw new NotAcceptableException("unsupported scope find");
            }
            var role = (await _apiDbContext.Roles.FindAsync(roleId)) ??
                throw new NotFoundException("role is not exist");
            var scopes = await _apiDbContext.Scopes
                .Where(s => s.RoleId == roleId).ToArrayAsync();
            _apiDbContext.Scopes.RemoveRange(scopes);
            var targets = await _apiDbContext.Scopes
                .Where(s => scopeNames.Contains(s.Name))
                .ToArrayAsync();
            role.Scopes = targets;
            _apiDbContext.Roles.Update(role);
            var result = await _apiDbContext.SaveChangesAsync();
            return result;
        }

        [ScopeDefinition("get all supported scopes", $"{ManagedResource.Role}.{ManagedAction.Read}.Scopes")]
        public IEnumerable<RoleScopeReadDto> GetScopes() =>
            Mapper.Map<IEnumerable<RoleScopeReadDto>>(RequireScopeUtil.Scopes);
    }
}