using Microsoft.Extensions.Configuration;

namespace MyScaffold.Core.Utilities
{
    public static class SettingUtil
    {
        public static void Initialize(IConfiguration configuration)
        {
            var jwtSetting = configuration.GetSection(nameof(Jwt));
            Jwt.Issuer = jwtSetting.GetValue<string>(nameof(Jwt.Issuer)) ??
                throw new Exception("missing issuer in jwt setting section");
            Jwt.KeyFolder = jwtSetting.GetValue<string>(nameof(Jwt.KeyFolder)) ??
                throw new Exception("missing key folder in jwt setting section");
            Jwt.Audience = jwtSetting.GetValue<string>(nameof(Jwt.Audience)) ??
                throw new Exception("missing audience in jwt setting section");
            Jwt.ExpireMin = TimeSpan.FromMinutes(jwtSetting.GetValue<int>(nameof(Jwt.ExpireMin)));
        }

        public static JwtSettings Jwt { get; private set; } = new JwtSettings();

        public static bool IsDevelopment { get; private set; } = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development";

        public class JwtSettings
        {
            public string KeyFolder { get; set; } = null!;
            public string Issuer { get; set; } = null!;
            public string Audience { get; set; } = null!;
            public TimeSpan ExpireMin { get; set; }
        }
    }
}