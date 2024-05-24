﻿using Microsoft.AspNetCore.Authorization;

namespace MyScaffold.Application.Auth.Requirements
{
    public class CustomRequireScope : IAuthorizationRequirement
    {
        public CustomRequireScope(string scope)
        {
            Scope = scope;
        }

        public string Scope { get; private set; }
    }
}