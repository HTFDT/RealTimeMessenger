using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations;

internal class MessageConfiguration : IEntityTypeConfiguration<Message>
{
    public void Configure(EntityTypeBuilder<Message> builder)
    {
        builder.ToTable("Messages");

        builder.HasOne(e => e.Group)
            .WithMany(e => e.Messages)
            .HasForeignKey(e => e.GroupId);

        builder.HasIndex(e => new { e.GroupId, e.GroupNumber })
            .IsUnique();

        builder.HasOne(e => e.Membership)
            .WithMany(e => e.Messages)
            .HasForeignKey(e => e.MembershipId);
    }
}