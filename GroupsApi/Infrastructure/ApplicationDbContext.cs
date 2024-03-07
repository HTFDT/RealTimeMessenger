using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure;

internal class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public DbSet<Group> Groups { get; set; } = null!;
    public DbSet<Tag> Tags { get; set; } = null!;
    public DbSet<GroupRole> GroupRoles { get; set; } = null!;
    public DbSet<Message> Messages { get; set; } = null!;
    public DbSet<GroupRight> GroupRights { get; set; } = null!;
    public DbSet<Membership> Memberships { get; set; } = null!;
    
    protected override void OnModelCreating(ModelBuilder modelBuilder) => 
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
    
}