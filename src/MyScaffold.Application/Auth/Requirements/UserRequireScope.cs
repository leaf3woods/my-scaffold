using Microsoft.AspNetCore.Authorization;

namespace MyScaffold.Application.Auth.Requirements
{
    public class UserRequireScope : IAuthorizationRequirement
    {
        public UserRequireScope(params string[] scopes)
        {
            Scope = ManagedResource.User + string.Join('.', scopes);
        }

        public string Scope { get; private set; }
    }
}