namespace EtAlii.Trends;

using EtAlii.Trends.Editor.Trends;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class ComponentEntityTypeConfiguration : IEntityTypeConfiguration<Component>
{
    public void Configure(EntityTypeBuilder<Component> builder)
    {
        // Use the builder to configure any rules that relate to the entity.
        builder.Property(entity => entity.Name).IsRequired();
        builder.Property(entity => entity.Moment).IsRequired();
    }
}
