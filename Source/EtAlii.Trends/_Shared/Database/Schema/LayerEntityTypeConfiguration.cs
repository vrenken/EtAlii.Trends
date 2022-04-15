namespace EtAlii.Trends;

using EtAlii.Trends.Editor.Layers;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class LayerEntityTypeConfiguration : IEntityTypeConfiguration<Layer>
{
    public void Configure(EntityTypeBuilder<Layer> builder)
    {
        builder.Property(e => e.Name).IsRequired();

        builder.HasOne(e=> e.Parent)
            .WithMany(e=> e.Children)
            .HasForeignKey(e => e.ParentId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
