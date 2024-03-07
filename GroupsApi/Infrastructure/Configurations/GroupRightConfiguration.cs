using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations;

internal class GroupRightConfiguration : IEntityTypeConfiguration<GroupRight>
{
    public void Configure(EntityTypeBuilder<GroupRight> builder)
    {
        builder.ToTable("GroupRights");
        
        builder.Property(e => e.NormalizedName)
            .HasComputedColumnSql("UPPER(\"Name\")", true);

        builder.HasIndex(e => e.NormalizedName)
            .IsUnique();
    }
}