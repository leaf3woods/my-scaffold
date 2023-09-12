using AutoMapper;
using Microsoft.Extensions.Logging;

namespace MyScaffold.Application.Services.Base
{
    public class BaseService : IBaseService
    {
        public IMapper Mapper { get; init; } = null!;
        public ILogger<BaseService> Logger { get; set; } = null!;
    }
}