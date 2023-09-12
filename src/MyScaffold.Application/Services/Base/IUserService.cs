using MyScaffold.Application.Dtos;

namespace MyScaffold.Application.Services.Base
{
    public interface IUserService : IBaseService
    {
        public Task<UserReadDto?> RegisterAsync(UserRegisterDto registerDto);

        public Task<string> LoginAsync(UserLoginDto credential);

        public Task LogoutAsync();

        public Task<int> DeleteAsync(Guid id);

        public Task<UserReadDto?> GetUserAsync(Guid id);

        public Task<IEnumerable<UserReadDto>> GetUsersWhereAsync(string? username = null);

        public Task<UserReadDto?> ChangeRoleAsync(Guid userId, Guid roleId);

        public Task<CaptchaReadDto> GenerateCaptchaAsync();

        public Task<int> ChangePasswordAsync(ChangePasswardDto passwardDto);

        public Task<int> ResetPasswordAsync(Guid userId);
    }
}