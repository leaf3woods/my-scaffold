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
                options.AddPolicy(scope.Name, policy =>
                    policy.AddRequirements(new CustomRequireScope(scope.Name)));
            }
        }
    }
}