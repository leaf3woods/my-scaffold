using AutoMapper;

namespace MyScaffold.Application.Services.Base
{
    public class BaseService : IBaseService
    {
        public IMapper Mapper { get; init; } = null!;
    }
}