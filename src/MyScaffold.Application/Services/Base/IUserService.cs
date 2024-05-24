﻿using MyScaffold.Application.Dtos;
using System.Security.Claims;

namespace MyScaffold.Application.Services.Base
{
    public interface IUserService : IBaseService
    {
        public Task<UserReadDto?> RegisterAsync(UserRegisterDto registerDto);

        public Task<string> LoginAsync(UserLoginDto credential);

        public Task LogoutAsync(IEnumerable<Claim> claims);

        public Task<int> DeleteAsync(Guid id);

        public Task<UserReadDto?> GetUserAsync(Guid id);

        public Task<IEnumerable<UserReadDto>> GetUsersWhereAsync(string? username = null);

        public Task<UserReadDto?> ChangeRoleAsync(Guid userId, Guid roleId);

        public Task<CaptchaReadDto> GenerateCaptchaAsync();

        public Task<int> ChangePasswordAsync(ChangePasswordDto passwordDto);

        public Task<int> ResetPasswordAsync(Guid userId);
    }
}