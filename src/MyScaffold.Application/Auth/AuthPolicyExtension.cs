using MyScaffold.Application.Auth.Requirements;
using MyScaffold.Domain.Entities;
using Microsoft.AspNetCore.Authorization;

namespace MyScaffold.Application.Auth
{
    public static class AuthPolicyExtension
    {
        public static void AddPolicyExt(this AuthorizationOptions options, IEnumerable<Scope> scopes)
        {
            foreach (var scope in scopes)
            {
                var parts = scope.Name.Split('.');
                switch (parts[0])
                {
                    case ManagedResource.User:
                        var urs = new UserRequireScope(parts[1..]);
                        options.AddPolicy(scope.Name, policy => policy.AddRequirements(urs));
                        break;

                    case ManagedResource.Role:
                        var rrs = new RoleRequireScope(parts[1..]);
                        options.AddPolicy(scope.Name, policy => policy.AddRequirements(rrs));
                        break;
                }
            }
        }
    }
}