namespace EtAlii.Trends;

using EtAlii.Trends.Diagrams;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class DiagramEntityTypeConfiguration : IEntityTypeConfiguration<Diagram>
{
    public void Configure(EntityTypeBuilder<Diagram> builder)
    {
        builder.Property(e => e.Name).IsRequired();

        builder
            .HasMany(entity => entity.Trends)
            .WithOne(entity => entity.Diagram)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasMany(entity => entity.Layers)
            .WithOne(entity => entity.Diagram)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

    }
}
