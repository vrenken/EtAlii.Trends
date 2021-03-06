namespace EtAlii.Trends;

using EtAlii.Trends.Editor.Trends;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class TrendEntityTypeConfiguration : IEntityTypeConfiguration<Trend>
{
    public void Configure(EntityTypeBuilder<Trend> builder)
    {
        // Use the builder to configure any rules that relate to the entity.
        builder.Property(entity => entity.Name).IsRequired();

        builder.HasOne(entity => entity.Diagram);

        builder
            .HasMany(entity => entity.Components)
            .WithOne(entity => entity.Trend)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

    }
}
