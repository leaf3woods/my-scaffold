﻿using MyScaffold.Core;
using MyScaffold.Core.Exceptions;
using MyScaffold.WebApi.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.Extensions.Localization;

namespace MyScaffold.WebApi.Utilities
{
    public static class ExceptionLocalizerExtension
    {
        public static async Task LocalizeException(HttpContext context, IStringLocalizer stringLocalizer)
        {
            context.Response.ContentType = "application/json";
            var exception = context.Features.Get<IExceptionHandlerFeature>();
            if (exception != null)
            {
                context.Response.StatusCode = exception.Error switch
                {
                    NotFoundException => StatusCodes.Status404NotFound,
                    NotAcceptableException => StatusCodes.Status406NotAcceptable,
                    _ => StatusCodes.Status500InternalServerError
                };

                var errDto = exception.Error.Localize(stringLocalizer);
                await context.Response.WriteAsJsonAsync(errDto, Options.CustomJsonSerializerOptions);
            }
        }
    }
}