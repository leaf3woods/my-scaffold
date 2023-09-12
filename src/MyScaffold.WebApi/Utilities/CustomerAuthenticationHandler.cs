using MyScaffold.Application.Services.Base;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using System.Text.Encodings.Web;

namespace MyScaffold.WebApi.Utilities
{
    public class CustomerAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        public CustomerAuthenticationHandler(
            IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory loggerFactory,
            UrlEncoder encoder,
            ISystemClock clock,
            IUserService userService
            ) : base(options, loggerFactory, encoder, clock)
        {
            _userService = userService;
        }

        private readonly IUserService _userService;

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            throw new NotImplementedException();
        }
    }
}