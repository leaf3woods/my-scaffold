using MyScaffold.Domain.Entities.Base;
using MyScaffold.Domain.Entities.Login;

namespace MyScaffold.Domain.Entities.Menus
{
    public class RoleMenu : IncrementEntity
    {
        public Guid MenuId { get; set; }

        public Menu? Menu { get; set; }

        public Guid RoleId {  get; set; }

        public Role? Role { get; set; }
    }
}
