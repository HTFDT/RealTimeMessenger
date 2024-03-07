using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations;

internal class GroupRoleConfiguration : IEntityTypeConfiguration<GroupRole>
{
    public void Configure(EntityTypeBuilder<GroupRole> builder)
    {
        builder.ToTable("GroupRoles");
        
        builder.Property(e => e.NormalizedName)
            .HasComputedColumnSql("UPPER(\"Name\")", true);

        builder.HasIndex(e => e.IsDefaultOwnerRole)
            .HasFilter("\"IsDefaultOwnerRole\" = true")
            .IsUnique();
        
        builder.HasIndex(e => e.IsDefaultMemberRole)
            .HasFilter("\"IsDefaultMemberRole\" = true")
            .IsUnique();
        
        builder.HasIndex(e => e.IsDefaultKickedRole)
            .HasFilter("\"IsDefaultKickedRole\" = true")
            .IsUnique();

        builder.HasIndex(e => e.IsDefaultLeftRole)
            .HasFilter("\"IsDefaultLeftRole\" = true")
            .IsUnique();

        builder.HasIndex(e => new { e.GroupId, e.NormalizedName })
            .IsUnique();
    }
}