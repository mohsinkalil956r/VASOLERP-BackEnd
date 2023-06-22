using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

using ERP.DAL.DB.Entities;
using System.Reflection.Emit;

namespace ERP.DAL.DB
{
    public class ERPContext : IdentityDbContext<SystemUser, Role, int, IdentityUserClaim<int>, UserRole,
        IdentityUserLogin<int>, IdentityRoleClaim<int>, IdentityUserToken<int>>
    {
        public ERPContext(DbContextOptions<ERPContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {

            //base.OnModelCreating(builder);

            builder.Entity<SystemUser>().ToTable("Users")
                .HasMany(e => e.Roles)
                .WithMany(e => e.Users)
                .UsingEntity<UserRole>();

            builder.Entity<Role>().ToTable("Roles");

            builder.Entity<UserRole>()
                .ToTable("UserRoles")
                .HasKey(x => new { x.UserId, x.RoleId });

            builder.Entity<IdentityUserClaim<int>>(entity => { entity.ToTable("UserClaims"); });

            builder.Entity<IdentityUserLogin<int>>(entity => 
            { 
                entity.ToTable("UserLogins");
                entity.HasKey(x => new { x.LoginProvider, x.ProviderKey });
            });

            builder.Entity<IdentityUserToken<int>>(entity => 
            { 
                entity.ToTable("UserTokens");
                entity.HasKey(x => new { x.UserId, x.LoginProvider, x.Name });
            });

            builder.Entity<IdentityRoleClaim<int>>(entity => { entity.ToTable("RoleClaims"); });

            builder.Entity<Permission>()
                .HasMany(e => e.Users)
                .WithMany(e => e.Permissions)
                .UsingEntity<UserPermission>();

            builder.Entity<Project>()
                .HasMany(e => e.Employees)
                .WithMany(e => e.Projects)
                .UsingEntity<ProjectEmployee>();

            builder.Entity<Asset>()
                .HasMany(e => e.Employees)
                .WithMany(e => e.Assets)
                .UsingEntity<AssetIssuance>();

            SeedData.Seed(builder);
        }

        public DbSet<Permission> Permissions { get; set; }
        public DbSet<UserPermission> UserPermissions { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<ProjectEmployee> ProjectEmployees { get; set; }
        public DbSet<Status> ProjectStatuses { get; set; }
        public DbSet<Asset> Assets { get; set; }
        public DbSet<AssetType> AssetType { get; set; }
        public DbSet<AssetIssuance> AssetIssuances { get; set; }
        public DbSet<ClientContact> ClientContacts { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<EmployeeContact> EmployeeContacts { get; set; }
        public DbSet<Expense> Expenses { get; set; }
        public DbSet<ExpenseType> ExpenseTypes { get; set; }
        public DbSet<PaymentMode> PaymentModes { get; set; }
    }
}
