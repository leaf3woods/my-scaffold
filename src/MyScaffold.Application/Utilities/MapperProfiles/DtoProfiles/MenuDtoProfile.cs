using AutoMapper;
using MyScaffold.Application.Dtos;
using MyScaffold.Domain.Entities.Authority;

namespace MyScaffold.Application.Utilities.MapperProfiles.DtoProfiles
{
    public class MenuDtoProfile : Profile
    {
        public MenuDtoProfile()
        {
            CreateMap<Menu, MenuReadDto>();
            CreateMap<MenuCreateDto, Menu>();
        }
    }
}
