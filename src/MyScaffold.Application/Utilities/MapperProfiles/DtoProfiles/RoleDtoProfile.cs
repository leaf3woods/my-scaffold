using AutoMapper;
using MyScaffold.Application.Dtos;
using MyScaffold.Domain.Entities;

namespace MyScaffold.Application.Utilities.MapperProfiles.DtoProfiles
{
    public class RoleDtoProfile : Profile
    {
        public RoleDtoProfile()
        {
            CreateMap<RoleCreateDto, Role>()
                .ForMember(dest => dest.Scopes, opt => opt.Ignore());
            CreateMap<Role, RoleReadDto>();
            CreateMap<Scope, RoleScopeReadDto>();
        }
    }
}