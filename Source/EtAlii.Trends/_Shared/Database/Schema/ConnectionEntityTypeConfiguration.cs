namespace EtAlii.Trends;

using EtAlii.Trends.Editor.Trends;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class ConnectionEntityTypeConfiguration : IEntityTypeConfiguration<Connection>
{
    public void Configure(EntityTypeBuilder<Connection> builder)
    {
        builder.HasOne(entity => entity.Diagram);

        builder
            .HasOne(entity => entity.Source);
        builder.Property(entity => entity.SourceBezierX).IsRequired();
        builder.Property(entity => entity.SourceBezierY).IsRequired();

        builder
            .HasOne(entity => entity.Target);
        builder.Property(entity => entity.TargetBezierX).IsRequired();
        builder.Property(entity => entity.TargetBezierY).IsRequired();
    }
}
