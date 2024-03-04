using Dal.Entities;
using Microsoft.EntityFrameworkCore;

namespace Dal.EfCore;

internal class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }


    public DbSet<ProfileDal> Profiles { get; set; } = null!;
    public DbSet<UserDal> Users { get; set; } = null!;
    
    public DbSet<RoleDal> Roles { get; set; } = null!;
    public DbSet<UserRoleDal> UsersRoles { get; set; } = null!;
    public DbSet<RefreshTokenDal> RefreshTokens { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<UserDal>(e =>
        {
            e.HasIndex(u => u.Username).IsUnique();
            e.HasMany(u => u.UserRoles)
                .WithOne(u => u.User)
                .HasForeignKey(ur => ur.UserId)
                .IsRequired();
            e.ToTable("Users");
        });

        modelBuilder.Entity<RoleDal>(e =>
        {
            e.HasIndex(r => r.Name).IsUnique();
            e.HasMany(r => r.UserRoles)
                .WithOne(r => r.Role)
                .HasForeignKey(ur => ur.RoleId)
                .IsRequired();
            e.ToTable("Roles");
        });
        
        modelBuilder.Entity<UserRoleDal>()
            .ToTable("UsersRoles")
            .HasKey(ur => new { ur.UserId, ur.RoleId });
        modelBuilder.Entity<ProfileDal>().ToTable("Profiles");
        modelBuilder.Entity<RefreshTokenDal>().ToTable("RefreshTokens");
    }
}