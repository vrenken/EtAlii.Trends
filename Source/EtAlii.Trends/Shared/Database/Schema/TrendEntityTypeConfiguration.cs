namespace EtAlii.Trends;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class TrendEntityTypeConfiguration : IEntityTypeConfiguration<Trend>
{
    public void Configure(EntityTypeBuilder<Trend> builder)
    {
        // Use the builder to configure any rules that relate to the entity.
        builder.Property(p => p.Name).IsRequired();
    }
}