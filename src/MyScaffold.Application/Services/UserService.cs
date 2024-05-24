using MyScaffold.Application.Auth;
using MyScaffold.Application.Captchas;
using MyScaffold.Application.Captchas.Builder;
using MyScaffold.Application.Dtos;
using MyScaffold.Application.Services.Base;
using MyScaffold.Application.Utilities;
using MyScaffold.Core;
using MyScaffold.Core.Exceptions;
using MyScaffold.Core.Utilities;
using MyScaffold.Domain.Entities;
using MyScaffold.Domain.Repositories;
using MyScaffold.Domain.Services;
using MyScaffold.Domain.Utilities;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace MyScaffold.Application.Services
{
    [ScopeDefinition("manage all user resources", ManagedResource.User)]
    public class UserService : BaseService, IUserService
    {
        public UserService(
            IUserRepository userRepository,
            IRoleRepository roleRepository,
            IUserDomainService userDomainService
            )
        {
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            _userDomainService = userDomainService;
        }

        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly IUserDomainService _userDomainService;

        public async Task<UserReadDto?> RegisterAsync(UserRegisterDto registerDto)
        {
            var user = Mapper.Map<User>(registerDto);
            var count = await _userRepository.CreateAsync(user);
            return count == 0 ? null : Mapper.Map<UserReadDto>(user);
        }

        public async Task<string> LoginAsync(UserLoginDto credential)
        {
            var answer = Mapper.Map<Captcha>(credential.Captcha);

            if (!SettingUtil.IsDevelopment
                && (answer is null || !await _userDomainService.VerifyCaptchaAnswerAsync(answer)))
            {
                throw new NotAcceptableException("captcha not found or not correct");
            }
            var user = await _userRepository.FindAsync(credential.Username);
            var bytes = Convert.FromBase64String(credential.Password);

            if (user is null || !user.Verify(bytes))
            {
                throw new NotAcceptableException("user not found or password error");
            }
            var token = JwtTokenUtil.GenerateJwtToken(SettingUtil.Jwt.Issuer, SettingUtil.Jwt.Audience, SettingUtil.Jwt.ExpireMin,
                new Claim(CustomClaimsType.UserId, user.Id.ToString()), new Claim(CustomClaimsType.RoleId, user.Role.Id.ToString())) ??
                throw new Exception("generate jwt token error");

            if (!await _userDomainService.VerifyTokenAsync(user.Id, token))
            {
                throw new ForbiddenException("user was logged in elsewhere");
            }
            await _userDomainService.CacheTokenAsync(user.Id, token);
            return token;
        }

        public async Task LogoutAsync(IEnumerable<Claim> claims)
        {
            var userId = claims.FirstOrDefault(c => c.Type == CustomClaimsType.UserId)!.Value;
            await _userDomainService.DeleteTokenAsync(Guid.Parse(userId)!);
        }

        [ScopeDefinition("delete user by id", $"{ManagedResource.User}.{ManagedAction.Delete}.One")]
        public async Task<int> DeleteAsync(Guid id)
        {
            return await _userRepository.DeleteAsync(id);
        }

        [ScopeDefinition("get single user by id", $"{ManagedResource.User}.{ManagedAction.Read}.One")]
        public async Task<UserReadDto?> GetUserAsync(Guid id)
        {
            var user = await _userRepository
                .GetQueryWhere(u => u.Id == id)
                .Include(u => u.Role)
                .FirstOrDefaultAsync();
            var result = Mapper.Map<UserReadDto>(user);
            return result;
        }

        [ScopeDefinition("get users where", $"{ManagedResource.User}.{ManagedAction.Read}.Query")]
        public async Task<IEnumerable<UserReadDto>> GetUsersWhereAsync(string? name = null)
        {
            var users = await _userRepository
                .GetQueryWhere(string.IsNullOrEmpty(name) ? null :
                u => u.Username.Contains(name) || u.DisplayName.Contains(name))
                .Include(u => u.Role)
                .ToArrayAsync();
            var results = Mapper.Map<IEnumerable<UserReadDto>>(users);
            return results;
        }

        [ScopeDefinition("change user role", $"{ManagedResource.User}.{ManagedAction.Update}.Role")]
        public async Task<UserReadDto?> ChangeRoleAsync(Guid userId, Guid roleId)
        {
            var user = (await _userRepository.FindAsync(userId)) ??
                throw new NotFoundException("user not found");
            if (await _roleRepository.FindAsync(roleId) is null)
                throw new NotFoundException("role not found");
            user.RoleId = roleId;
            var count = await _userRepository.UpdateAsync(user);
            var result = count == 0 ? null : Mapper.Map<UserReadDto>(user);
            return result;
        }

        public async Task<CaptchaReadDto> GenerateCaptchaAsync()
        {
            //var builder = CaptchaBuilder.Create<CharacterCaptchaBuilder>()
            //    .WithLowerCase()
            //    .WithUpperCase();
            var builder = CaptchaBuilder.Create<QuestionCaptchaBuilder>();
            var captcha = builder.WithGenOption(new CaptchaGenOptions
            {
                FontFamily = "consolas",
                Height = 80,
                Width = 200,
            }).WithNoise().WithLines().WithCircles().Build();
            await _userDomainService.CacheCaptchaAnswerAsync(captcha, 180);
            return Mapper.Map<CaptchaReadDto>(captcha);
        }

        public async Task<int> ChangePasswordAsync(ChangePasswardDto passwardDto)
        {
            var answer = Mapper.Map<Captcha>(passwardDto.Captcha);

            if (!SettingUtil.IsDevelopment
                && (answer is null || !await _userDomainService.VerifyCaptchaAnswerAsync(answer)))
            {
                throw new NotAcceptableException("captcha not exist or not correct");
            }
            var user = await _userRepository.FindAsync(passwardDto.Username) ?? throw new NotFoundException("user not found");
            var bytes = Convert.FromBase64String(passwardDto.OldPassword);
            if (!user.Verify(bytes))
            {
                throw new NotAcceptableException("password error");
            }

            _userDomainService.WithSalt(ref user, passwardDto.NewPassword);
            return await _userRepository.UpdateAsync(user);
        }

        [ScopeDefinition("reset someone's password", $"{ManagedResource.User}.{ManagedAction.Update}.ResetPwd")]
        public async Task<int> ResetPasswordAsync(Guid userId)
        {
            var user = await _userRepository.FindAsync(userId) ??
                throw new NotFoundException("user not found");
            var hash = CryptoUtil.Sha256("12345678");
            _userDomainService.WithSalt(ref user, hash);
            return await _userRepository.UpdateAsync(user);
        }
    }
}