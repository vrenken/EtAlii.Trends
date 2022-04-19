namespace EtAlii.Trends;

using EtAlii.Trends.Editor.Trends;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class ConnectionEntityTypeConfiguration : IEntityTypeConfiguration<Connection>
{
    public void Configure(EntityTypeBuilder<Connection> builder)
    {
        // builder.Property(entity => entity.From).IsRequired();
        // builder.Property(entity => entity.To).IsRequired();

        // builder
        //     .HasOne(entity => entity.From)
        //     .WithMany(entity => entity.Connections)
        //     .IsRequired();
            //.OnDelete(DeleteBehavior.SetNull);

        builder.HasOne(entity => entity.Diagram);

        builder
            .HasOne(entity => entity.Source);
//            .IsRequired();

        builder
            .HasOne(entity => entity.Target);
  //          .IsRequired();
            //.HasOne(entity => entity.To)

            //.WithMany(entity => entity.Connections)
            //.IsRequired();
            //.OnDelete(DeleteBehavior.Cascade);

    }
}
