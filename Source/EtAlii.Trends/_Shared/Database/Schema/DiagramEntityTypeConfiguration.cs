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
            .HasMany(e => e.Trends)
            .WithOne(e => e.Diagram)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasMany(e => e.Layers)
            .WithOne(e => e.Diagram)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

    }
}
