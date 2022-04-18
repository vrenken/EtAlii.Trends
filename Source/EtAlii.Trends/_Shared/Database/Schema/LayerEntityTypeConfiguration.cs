namespace EtAlii.Trends;

using EtAlii.Trends.Editor.Layers;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class LayerEntityTypeConfiguration : IEntityTypeConfiguration<Layer>
{
    public void Configure(EntityTypeBuilder<Layer> builder)
    {
        builder.Property(entity => entity.Name).IsRequired();

        builder.HasOne(entity => entity.Diagram);

        builder.HasOne(entity=> entity.Parent)
            .WithMany(entity=> entity.Children)
            .HasForeignKey(entity => entity.ParentId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
