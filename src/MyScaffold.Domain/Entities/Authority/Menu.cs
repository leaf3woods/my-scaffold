using MyScaffold.Domain.Entities.Base;

namespace MyScaffold.Domain.Entities.Authority
{
    public class Menu : UniversalEntity, ISoftDelete
    {
        public string Name { get; set; } = null!;

        public string Code { get; set; } = null!;

        public string Description { get; set; } = null!;

        public Guid? ParentId { get; set; }

        public int Type {  get; set; }

        public int Order { get; set; }

        public string Path { get; set; } = null!;

        public string IconUrl { get; set; } = null!;

        public string Route { get; set; } = null!;

        public bool Visible { get; set; }

        public bool Favorite { get; set; }

        #region delete filter

        public bool SoftDeleted { get; set; } = false;
        public DateTime? DeleteTime { get; set; } = null;

        #endregion delete filter

    }
}
