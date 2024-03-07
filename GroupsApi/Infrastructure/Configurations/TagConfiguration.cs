using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations;

internal class TagConfiguration: IEntityTypeConfiguration<Tag>
{
    public void Configure(EntityTypeBuilder<Tag> builder)
    {
        builder.ToTable("Tags");

        builder.HasIndex(e => e.NormalizedName)
            .IsUnique();

        builder.Property(e => e.NormalizedName)
            .HasComputedColumnSql("UPPER(\"Name\")", true);
    }
}