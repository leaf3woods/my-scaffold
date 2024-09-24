﻿using MyScaffold.Domain.Entities.Login;

namespace MyScaffold.Domain.Services
{
    public interface IUserDomainService : IDomainService
    {
        public Task CacheCaptchaAnswerAsync(Captcha captcha, int expire);

        public Task CacheTokenAsync(Guid userId, string token);

        public Task<bool> VerifyCaptchaAnswerAsync(Captcha captcha);

        public Task<bool> VerifyTokenAsync(Guid userId, string token);

        public Task<bool> DeleteTokenAsync(Guid userId);

        public void WithSalt(ref User user, string password);
    }
}