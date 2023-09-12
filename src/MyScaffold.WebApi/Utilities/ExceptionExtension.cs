using MyScaffold.Application.Dtos;
using MyScaffold.Core.Exceptions;
using MyScaffold.Core.Utilities;
using Microsoft.Extensions.Localization;

namespace MyScaffold.WebApi.Exceptions
{
    public static class ExceptionExtension
    {
        public static ExceptionReadDto Localize(this Exception exception, IStringLocalizer stringLocalizer)
        {
            var index = exception.Message.IndexOf('\r');
            var result = exception switch
            {
                NotFoundException or NotAcceptableException or ForbiddenException => new ExceptionReadDto() { Info = stringLocalizer[(exception as CustomException)!.ExceptionCode] },
                _ => SettingUtil.IsDevelopment ? new ExceptionReadDto
                {
                    Info = exception.Message,
                    StackTrace = exception.StackTrace,
                    Inner = exception.InnerException?.Message
                } : new ExceptionReadDto
                {
                    Info = index == -1 ? exception.Message : exception.Message[..index],
                    StackTrace = null,
                    Inner = null
                }
            };
            return result;
        }
    }
}