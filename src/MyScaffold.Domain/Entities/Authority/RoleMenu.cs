﻿using MyScaffold.Domain.Entities.Base;
using MyScaffold.Domain.Entities.Login;

namespace MyScaffold.Domain.Entities.Authority
{
    public class RoleMenu : IncrementEntity
    {
        public Guid MenuId { get; set; }

        public Menu Menu { get; set; } = null!;

        public Guid RoleId { get; set; }

        public Role Role { get; set; } = null!;
    }
}
