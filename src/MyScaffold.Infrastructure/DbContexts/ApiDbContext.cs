using MyScaffold.Domain.Entities;
using MyScaffold.Domain.Entities.Base;
using Microsoft.EntityFrameworkCore;
using CaseExtensions;
using MyScaffold.Domain.Entities.Login;
using MyScaffold.Domain.Entities.Menus;

namespace MyScaffold.Infrastructure.DbContexts
{
    public class ApiDbContext : DbContext
    {
        public ApiDbContext(DbContextOptions<ApiDbContext> options) : base(options)
        {
        }

        #region dbsets

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Scope> Scopes { get; set; }

        #endregion dbsets

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            foreach (var entityEntry in ChangeTracker.Entries())
            {
                if (entityEntry.State == EntityState.Deleted && entityEntry.Entity is ISoftDelete delete)
                {
                    entityEntry.State = EntityState.Unchanged;
                    delete.SoftDeleted = true;
                    delete.DeleteTime = DateTime.UtcNow;
                }
            }
            return base.SaveChangesAsync(cancellationToken);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            #region Table prefix

            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                entityType.SetTableName(entityType.ClrType.Name.ToSnakeCase());
            }

            #endregion Table prefix

            #region soft delete filter

            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                if (typeof(ISoftDelete).IsAssignableFrom(entityType.ClrType))
                {
                    entityType.AddSoftDeleteQueryFilter();
                }
            }

            #endregion soft delete filter

            #region entities initialize

            #region role

            modelBuilder.Entity<Role>()
                .HasData(Role.Seeds);

            modelBuilder.Entity<Role>()
                .HasIndex(u => u.SoftDeleted);

            modelBuilder.Entity<Role>()
                .HasIndex(r => new { r.Name, r.SoftDeleted })
                .IsUnique();

            modelBuilder.Entity<Role>()
                .HasMany(r => r.Scopes)
                .WithOne(s => s.Role)
                .IsRequired(false)
                .HasForeignKey(s => s.RoleId);

            modelBuilder.Entity<Role>()
                .HasMany(u => u.Users)
                .WithOne(b => b.Role)
                .HasForeignKey(b => b.RoleId);

            modelBuilder.Entity<Role>()
                .HasMany(r => r.Menus)
                .WithMany()
                .UsingEntity<RoleMenu>();

            #endregion role

            #region scope

            modelBuilder.Entity<Scope>()
                .HasData(Scope.Seeds);

            #endregion scope

            #region user

            modelBuilder.Entity<User>()
                .HasData(User.Seeds);

            modelBuilder.Entity<User>()
                .HasIndex(u => new { u.Username, u.SoftDeleted })
                .IsUnique();

            modelBuilder.Entity<User>()
                .HasIndex(u => u.SoftDeleted);

            modelBuilder.Entity<User>()
                .OwnsOne(u => u.Settings);

            modelBuilder.Entity<User>()
                .OwnsOne(u => u.Detail);

            #endregion user

            #region menu

            modelBuilder.Entity<Menu>()
                .HasIndex(m => m.Order);

            #endregion

            #endregion entities initialize
        }
    }
}