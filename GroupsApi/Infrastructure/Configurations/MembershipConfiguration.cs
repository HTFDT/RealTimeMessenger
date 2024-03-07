using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations;

internal class MembershipConfiguration : IEntityTypeConfiguration<Membership>
{
    public void Configure(EntityTypeBuilder<Membership> builder)
    {
        builder.ToTable("Memberships");

        builder.HasIndex(e => new { e.UserId, e.GroupId })
            .IsUnique();

        builder.HasIndex(e => new { e.GroupRoleId, e.GroupId, e.IsRoleUnique })
            .HasFilter("\"IsRoleUnique\" = true")
            .IsUnique();

        builder.HasOne(e => e.GroupRole)
            .WithMany(e => e.Memberships)
            .HasForeignKey(e => e.GroupRoleId)
            .OnDelete(DeleteBehavior.SetNull)
            .IsRequired(false);
        
        builder.HasOne(e => e.GroupRoleBeforeLeave)
            .WithMany(e => e.MembershipsWithThisRoleBeforeLeave)
            .HasForeignKey(e => e.GroupRoleBeforeLeaveId)
            .OnDelete(DeleteBehavior.SetNull)
            .IsRequired(false);

        builder.HasOne(e => e.Group)
            .WithMany(e => e.Memberships)
            .HasForeignKey(e => e.GroupId);
    }
}